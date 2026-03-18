using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_VillarrealHugo.Data;
using ProyectoFinal_VillarrealHugo.Models;

namespace ProyectoFinal_VillarrealHugo.Controllers
{
	public class CarrerasController : Controller
	{
		private readonly AppDbContext _context;

		public CarrerasController(AppDbContext context)
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

			var lista = await _context.Carreras.ToListAsync();
			return View(lista);
		}

		public IActionResult Create()
		{
			if (!AdminLogueado())
				return RedirectToAction("Login", "AdminAuth");

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Carrera carrera)
		{
			if (!AdminLogueado())
				return RedirectToAction("Login", "AdminAuth");

			if (!ModelState.IsValid)
				return View(carrera);

			_context.Carreras.Add(carrera);
			await _context.SaveChangesAsync();

			TempData["Success"] = "Carrera creada correctamente";
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Edit(int id)
		{
			if (!AdminLogueado())
				return RedirectToAction("Login", "AdminAuth");

			var carrera = await _context.Carreras.FindAsync(id);

			if (carrera == null)
				return NotFound();

			return View(carrera);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Carrera carrera)
		{
			if (!AdminLogueado())
				return RedirectToAction("Login", "AdminAuth");

			if (id != carrera.Id)
				return NotFound();

			if (!ModelState.IsValid)
				return View(carrera);

			try
			{
				_context.Update(carrera);
				await _context.SaveChangesAsync();

				TempData["Success"] = "Carrera actualizada correctamente";
			}
			catch (DbUpdateException)
			{
				TempData["Error"] = "Error al actualizar la carrera";
			}

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(int id)
		{
			if (!AdminLogueado())
				return RedirectToAction("Login", "AdminAuth");

			var carrera = await _context.Carreras.FindAsync(id);

			if (carrera == null)
				return NotFound();

			return View(carrera);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (!AdminLogueado())
				return RedirectToAction("Login", "AdminAuth");

			var carrera = await _context.Carreras.FindAsync(id);

			if (carrera == null)
				return NotFound();

			try
			{
				_context.Carreras.Remove(carrera);
				await _context.SaveChangesAsync();

				TempData["Success"] = "Carrera eliminada correctamente";
			}
			catch (DbUpdateException)
			{
				TempData["Error"] = "No se puede eliminar (puede tener cursos asociados)";
			}

			return RedirectToAction(nameof(Index));
		}
	}
}