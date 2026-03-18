using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_VillarrealHugo.Data;

namespace ProyectoFinal_VillarrealHugo.Controllers
{
	public class HomeController : Controller
	{
		private readonly AppDbContext _context;

		public HomeController(AppDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			if (HttpContext.Session.GetString("Admin") != null)
				return RedirectToAction("Dashboard", "Admin");

			if (HttpContext.Session.GetString("Estudiante") != null)
				return RedirectToAction("Inicio", "Estudiantes");

			ViewBag.TotalCarreras = _context.Carreras.Count();
			ViewBag.TotalCursos = _context.Cursos.Count();

			return View();
		}
	}
}