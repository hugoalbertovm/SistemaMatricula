using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_VillarrealHugo.Models
{
	public class Carrera
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Nombre { get; set; } = string.Empty;

		[Range(1, 10)]
		public int Duracion { get; set; }

		public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
	}
}