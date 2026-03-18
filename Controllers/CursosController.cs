using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_VillarrealHugo.Data;
using ProyectoFinal_VillarrealHugo.Models;

namespace ProyectoFinal_VillarrealHugo.Controllers
{
	public class CursosController : Controller
	{
		private readonly AppDbContext _context;

		public CursosController(AppDbContext context)
		{
			_context = context;
		}

		private bool AdminLogueado()
		{
			return HttpContext.Session.GetString("Admin") != null;
		}

		public async Task<IActionResult> Index()
		{
			if (!AdminLogueado())
				return RedirectToAction("Login", "AdminAuth");

			var cursos = _context.Cursos
				.Include(c => c.Carrera)
				.Include(c => c.Docente);

			return View(await cursos.ToListAsync());
		}

		public IActionResult Create()
		{
			if (!AdminLogueado())
				return RedirectToAction("Login", "AdminAuth");

			ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre");
			ViewData["DocenteId"] = new SelectList(_context.Docentes, "Id", "Nombre");

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Curso curso)
		{
			if (ModelState.IsValid)
			{
				_context.Add(curso);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre", curso.CarreraId);
			ViewData["DocenteId"] = new SelectList(_context.Docentes, "Id", "Nombre", curso.DocenteId);

			return View(curso);
		}
	}
}