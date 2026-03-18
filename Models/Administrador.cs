using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_VillarrealHugo.Models
{
	public class Administrador
	{
		public int Id { get; set; }

		[Required]
		[EmailAddress]
		public string Correo { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;
	}
}