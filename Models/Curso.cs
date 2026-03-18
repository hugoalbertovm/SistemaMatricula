using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal_VillarrealHugo.Models
{
	public class Curso
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Nombre { get; set; } = string.Empty;

		[Range(1, 10)]
		public int Creditos { get; set; }

		[Required]
		public int CarreraId { get; set; }

		[Required]
		public int DocenteId { get; set; }

		public Carrera? Carrera { get; set; }

		public Docente? Docente { get; set; }
	}
}