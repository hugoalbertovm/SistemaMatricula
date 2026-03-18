using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_VillarrealHugo.Data;
using ProyectoFinal_VillarrealHugo.Models;

namespace ProyectoFinal_VillarrealHugo.Controllers
{
	public class DetalleMatriculaController : Controller
	{
		private readonly AppDbContext _context;

		public DetalleMatriculaController(AppDbContext context)
		{
			_context = context;
		}

		private bool Logueado()
		{
			return HttpContext.Session.GetString("Estudiante") != null;
		}

		public IActionResult SeleccionarCursos(int matriculaId)
		{
			if (!Logueado())
				return RedirectToAction("Login", "Auth");

			ViewBag.MatriculaId = matriculaId;

			var cursos = _context.Cursos.ToList();

			return View(cursos);
		}

		[HttpPost]
		public IActionResult GuardarCursos(int matriculaId, List<int> cursosSeleccionados)
		{
			if (!Logueado())
				return RedirectToAction("Login", "Auth");

			if (cursosSeleccionados == null || !cursosSeleccionados.Any())
			{
				ModelState.AddModelError("", "Debe seleccionar al menos un curso");
				ViewBag.MatriculaId = matriculaId;
				return View("SeleccionarCursos", _context.Cursos.ToList());
			}

			foreach (var cursoId in cursosSeleccionados)
			{
				var existe = _context.DetalleMatriculas
					.Any(d => d.MatriculaId == matriculaId && d.CursoId == cursoId);

				if (!existe)
				{
					_context.DetalleMatriculas.Add(new DetalleMatricula
					{
						MatriculaId = matriculaId,
						CursoId = cursoId
					});
				}
			}

			_context.SaveChanges();

			return RedirectToAction("Index", "Matriculas");
		}
	}
}