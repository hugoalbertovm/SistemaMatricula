using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_VillarrealHugo.Data;

namespace ProyectoFinal_VillarrealHugo.Controllers
{
	public class AdminController : Controller
	{
		private readonly AppDbContext _context;

		public AdminController(AppDbContext context)
		{
			_context = context;
		}
		private bool AdminLogueado()
		{
			return HttpContext.Session.GetString("Admin") != null;
		}

		public IActionResult Dashboard()
		{
			if (!AdminLogueado())
				return RedirectToAction("Login", "AdminAuth");

			ViewBag.Carreras = _context.Carreras.Count();
			ViewBag.Cursos = _context.Cursos.Count();
			ViewBag.Estudiantes = _context.Estudiantes.Count();

			ViewBag.Matriculas = _context.Matriculas.Count();
			ViewBag.Pagadas = _context.Matriculas.Count(m => m.EstadoPago == "Pagado");
			ViewBag.Pendientes = _context.Matriculas.Count(m => m.EstadoPago == "Pendiente");

			return View();
		}

		public async Task<IActionResult> Matriculas()
		{
			if (!AdminLogueado())
				return RedirectToAction("Login", "AdminAuth");

			var lista = await _context.Matriculas
				.Include(m => m.Estudiante)
				.Include(m => m.Detalles)
					.ThenInclude(d => d.Curso)
				.OrderByDescending(m => m.Fecha)
				.ToListAsync();

			return View(lista);
		}

		public async Task<IActionResult> Detalle(int id)
		{
			if (!AdminLogueado())
				return RedirectToAction("Login", "AdminAuth");

			var matricula = await _context.Matriculas
				.Include(m => m.Estudiante)
				.Include(m => m.Detalles)
					.ThenInclude(d => d.Curso)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (matricula == null)
				return NotFound();

			return View(matricula);
		}

		[HttpPost]
		public async Task<IActionResult> Eliminar(int id)
		{
			if (!AdminLogueado())
				return RedirectToAction("Login", "AdminAuth");

			var matricula = await _context.Matriculas
				.Include(m => m.Detalles)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (matricula == null)
				return NotFound();

			if (matricula.EstadoPago == "Pagado")
			{
				TempData["Error"] = "No se puede eliminar una matrícula pagada";
				return RedirectToAction("Matriculas");
			}

			_context.DetalleMatriculas.RemoveRange(matricula.Detalles);

			_context.Matriculas.Remove(matricula);

			await _context.SaveChangesAsync();

			TempData["Success"] = "Matrícula eliminada correctamente";

			return RedirectToAction("Matriculas");
		}
	}
}