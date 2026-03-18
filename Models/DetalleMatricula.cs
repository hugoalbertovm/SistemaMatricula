using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_VillarrealHugo.Models
{
	public class DetalleMatricula
	{
		public int Id { get; set; }

		[Required]
		public int MatriculaId { get; set; }

		[Required]
		public int CursoId { get; set; }

		public Matricula? Matricula { get; set; }

		public Curso? Curso { get; set; }
	}
}