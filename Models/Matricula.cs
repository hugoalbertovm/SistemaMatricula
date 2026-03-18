using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal_VillarrealHugo.Models
{
	public class Matricula
	{
		public int Id { get; set; }

		[Required]
		public int EstudianteId { get; set; }

		public Estudiante? Estudiante { get; set; }

		[Required]
		public DateTime Fecha { get; set; } = DateTime.Now;

		[Column(TypeName = "decimal(10,2)")]
		public decimal Total { get; set; } = 0;

		[Required]
		[StringLength(20)]
		public string Estado { get; set; } = "Activa";

		[Required]
		[StringLength(20)]
		public string EstadoPago { get; set; } = "Pendiente";

		[StringLength(50)]
		public string? Comprobante { get; set; }

		public ICollection<DetalleMatricula> Detalles { get; set; } = new List<DetalleMatricula>();
	}
}