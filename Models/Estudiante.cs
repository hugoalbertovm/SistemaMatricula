using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_VillarrealHugo.Models
{
	public class Estudiante
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre es obligatorio")]
		[StringLength(100)]
		public string Nombre { get; set; } = string.Empty;

		[Required(ErrorMessage = "El correo es obligatorio")]
		[EmailAddress(ErrorMessage = "Correo inválido")]
		public string Correo { get; set; } = string.Empty;

		[Required(ErrorMessage = "La contraseña es obligatoria")]
		[StringLength(50, MinimumLength = 4)]
		public string Password { get; set; } = string.Empty;
	}
}