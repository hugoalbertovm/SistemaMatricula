using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_VillarrealHugo.Models
{
	public class Docente
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Nombre { get; set; } = string.Empty;

		[Required]
		[StringLength(100)]
		public string Especialidad { get; set; } = string.Empty;

		[Required]
		[EmailAddress]
		public string Correo { get; set; } = string.Empty;
	}
}