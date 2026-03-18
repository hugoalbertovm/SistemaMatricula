using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_VillarrealHugo.Data;
using ProyectoFinal_VillarrealHugo.Models;

namespace ProyectoFinal_VillarrealHugo.Controllers
{
	public class AuthController : Controller
	{
		private readonly AppDbContext _context;

		public AuthController(AppDbContext context)
		{
			_context = context;
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(string correo, string password)
		{
			var user = _context.Estudiantes
				.FirstOrDefault(u => u.Correo == correo && u.Password == password);

			if (user != null)
			{
				HttpContext.Session.SetString("Estudiante", user.Correo);
				return RedirectToAction("Inicio", "Estudiante");
			}

			ViewBag.Error = "Credenciales incorrectas";
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Register(Estudiante estudiante)
		{
			if (ModelState.IsValid)
			{
				_context.Estudiantes.Add(estudiante);
				_context.SaveChanges();
				return RedirectToAction("Login");
			}

			return View(estudiante);
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Login");
		}
	}
}