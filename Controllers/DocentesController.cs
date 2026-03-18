using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_VillarrealHugo.Data;
using ProyectoFinal_VillarrealHugo.Models;

namespace ProyectoFinal_VillarrealHugo.Controllers
{
	public class DocentesController : Controller
	{
		private readonly AppDbContext _context;

		public DocentesController(AppDbContext context)
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

			return View(await _context.Docentes.ToListAsync());
		}

		public IActionResult Create()
		{
			if (!AdminLogueado())
				return RedirectToAction("Login", "AdminAuth");

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Docente docente)
		{
			if (ModelState.IsValid)
			{
				_context.Add(docente);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return View(docente);
		}
	}
}