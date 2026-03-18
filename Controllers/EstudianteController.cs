using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_VillarrealHugo.Data;

namespace ProyectoFinal_VillarrealHugo.Controllers
{
	public class EstudianteController : Controller
	{
		private readonly AppDbContext _context;

		public EstudianteController(AppDbContext context)
		{
			_context = context;
		}

		private bool Logueado()
		{
			return HttpContext.Session.GetString("Estudiante") != null;
		}

		public IActionResult Inicio()
		{
			if (!Logueado())
				return RedirectToAction("Login", "Auth");

			var carreras = _context.Carreras.ToList();
			return View(carreras);
		}

		public IActionResult CursosPorCarrera(int id)
		{
			if (!Logueado())
				return RedirectToAction("Login", "Auth");

			var cursos = _context.Cursos
				.Include(c => c.Docente)
				.Where(c => c.CarreraId == id)
				.ToList();

			return View(cursos);
		}
	}
}