using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_VillarrealHugo.Data;

namespace ProyectoFinal_VillarrealHugo.Controllers
{
	public class AdminAuthController : Controller
	{
		private readonly AppDbContext _context;

		public AdminAuthController(AppDbContext context)
		{
			_context = context;
		}

		public IActionResult Login()
		{
			if (HttpContext.Session.GetString("Admin") != null)
			{
				return RedirectToAction("Dashboard", "Admin");
			}

			return View();
		}

		[HttpPost]
		public IActionResult Login(string correo, string password)
		{
			if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(password))
			{
				ViewBag.Error = "Debe ingresar correo y contraseña";
				return View();
			}

			var admin = _context.Administradores
				.FirstOrDefault(a => a.Correo == correo && a.Password == password);

			if (admin != null)
			{
				HttpContext.Session.SetString("Admin", admin.Correo);

				return RedirectToAction("Dashboard", "Admin");
			}

			ViewBag.Error = "Credenciales incorrectas";
			return View();
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Login");
		}
	}
}