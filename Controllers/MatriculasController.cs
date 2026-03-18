using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_VillarrealHugo.Data;
using ProyectoFinal_VillarrealHugo.Models;

namespace ProyectoFinal_VillarrealHugo.Controllers
{
	public class MatriculasController : Controller
	{
		private readonly AppDbContext _context;

		public MatriculasController(AppDbContext context)
		{
			_context = context;
		}

		private bool Logueado()
		{
			return HttpContext.Session.GetString("Estudiante") != null;
		}

		private async Task<Estudiante?> ObtenerEstudiante()
		{
			var correo = HttpContext.Session.GetString("Estudiante");

			return await _context.Estudiantes
				.FirstOrDefaultAsync(e => e.Correo == correo);
		}

		public async Task<IActionResult> Index()
		{
			if (!Logueado())
				return RedirectToAction("Login", "Auth");

			var estudiante = await ObtenerEstudiante();

			if (estudiante == null)
				return RedirectToAction("Login", "Auth");

			var matriculas = await _context.Matriculas
				.Include(m => m.Detalles)
					.ThenInclude(d => d.Curso)
				.Where(m => m.EstudianteId == estudiante.Id)
				.OrderByDescending(m => m.Fecha)
				.ToListAsync();

			return View(matriculas);
		}

		[HttpPost]
		public async Task<IActionResult> Create(int cursoId)
		{
			if (!Logueado())
				return RedirectToAction("Login", "Auth");

			var estudiante = await ObtenerEstudiante();

			if (estudiante == null)
				return RedirectToAction("Login", "Auth");

			var curso = await _context.Cursos.FindAsync(cursoId);

			if (curso == null)
			{
				TempData["Error"] = "El curso no existe";
				return RedirectToAction("Index", "Estudiantes");
			}

			var matricula = await _context.Matriculas
				.Include(m => m.Detalles)
				.FirstOrDefaultAsync(m =>
					m.EstudianteId == estudiante.Id &&
					m.Estado == "Activa");

			if (matricula == null)
			{
				matricula = new Matricula
				{
					EstudianteId = estudiante.Id,
					Fecha = DateTime.Now,
					Estado = "Activa",
					EstadoPago = "Pendiente",
					Total = 0
				};

				_context.Matriculas.Add(matricula);
				await _context.SaveChangesAsync();
			}

			bool yaExiste = await _context.DetalleMatriculas
				.AnyAsync(d => d.MatriculaId == matricula.Id && d.CursoId == cursoId);

			if (yaExiste)
			{
				TempData["Error"] = "Este curso ya está matriculado";
				return RedirectToAction("Index");
			}

			var detalle = new DetalleMatricula
			{
				MatriculaId = matricula.Id,
				CursoId = cursoId
			};

			_context.DetalleMatriculas.Add(detalle);

			matricula.Total += curso.Creditos;

			await _context.SaveChangesAsync();

			TempData["Success"] = "Curso agregado correctamente";

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Editar(int id)
		{
			if (!Logueado())
				return RedirectToAction("Login", "Auth");

			var matricula = await _context.Matriculas
				.Include(m => m.Detalles)
					.ThenInclude(d => d.Curso)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (matricula == null)
				return NotFound();

			return View(matricula);
		}

		[HttpPost]
		public async Task<IActionResult> EliminarCurso(int detalleId)
		{
			var detalle = await _context.DetalleMatriculas
				.FirstOrDefaultAsync(d => d.Id == detalleId);

			if (detalle == null)
				return NotFound();

			var matricula = await _context.Matriculas
				.FindAsync(detalle.MatriculaId);

			var curso = await _context.Cursos
				.FindAsync(detalle.CursoId);

			if (matricula != null && matricula.EstadoPago == "Pagado")
			{
				TempData["Error"] = "No puedes modificar una matrícula pagada";
				return RedirectToAction("Index");
			}

			_context.DetalleMatriculas.Remove(detalle);

			if (curso != null && matricula != null)
			{
				matricula.Total -= curso.Creditos;
			}

			await _context.SaveChangesAsync();

			return RedirectToAction("Editar", new { id = detalle.MatriculaId });
		}

		[HttpPost]
		public async Task<IActionResult> EliminarMatricula(int id)
		{
			var matricula = await _context.Matriculas
				.Include(m => m.Detalles)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (matricula == null)
				return NotFound();

			if (matricula.EstadoPago == "Pagado")
			{
				TempData["Error"] = "No puedes eliminar una matrícula pagada";
				return RedirectToAction("Index");
			}

			_context.DetalleMatriculas.RemoveRange(matricula.Detalles);
			_context.Matriculas.Remove(matricula);

			await _context.SaveChangesAsync();

			TempData["Success"] = "Matrícula eliminada correctamente";

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Confirmacion(int id)
		{
			if (!Logueado())
				return RedirectToAction("Login", "Auth");

			var matricula = await _context.Matriculas
				.Include(m => m.Detalles)
					.ThenInclude(d => d.Curso)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (matricula == null)
				return NotFound();

			return View(matricula);
		}

		[HttpPost]
		public async Task<IActionResult> Pagar(int id)
		{
			var matricula = await _context.Matriculas.FindAsync(id);

			if (matricula == null)
				return NotFound();

			if (matricula.EstadoPago == "Pagado")
			{
				TempData["Error"] = "Esta matrícula ya fue pagada";
				return RedirectToAction("Index");
			}

			matricula.EstadoPago = "Pagado";
			matricula.Estado = "Confirmada";
			matricula.Comprobante = "MAT-" + DateTime.Now.Ticks;

			await _context.SaveChangesAsync();

			TempData["Success"] = "Pago realizado correctamente";

			return RedirectToAction("Confirmacion", new { id });
		}

		[HttpPost]
		public async Task<IActionResult> Confirmar(int id)
		{
			var matricula = await _context.Matriculas.FindAsync(id);

			if (matricula == null)
				return NotFound();

			matricula.Estado = "Confirmada";

			await _context.SaveChangesAsync();

			return RedirectToAction("Index");
		}
	}
}