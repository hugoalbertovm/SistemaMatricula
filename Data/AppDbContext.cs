using Microsoft.EntityFrameworkCore;
using ProyectoFinal_VillarrealHugo.Models;

namespace ProyectoFinal_VillarrealHugo.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Carrera> Carreras { get; set; }
		public DbSet<Curso> Cursos { get; set; }
		public DbSet<Docente> Docentes { get; set; }
		public DbSet<Estudiante> Estudiantes { get; set; }
		public DbSet<Matricula> Matriculas { get; set; }
		public DbSet<DetalleMatricula> DetalleMatriculas { get; set; }
		public DbSet<Administrador> Administradores { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// ================= CURSO =================
			modelBuilder.Entity<Curso>()
				.HasOne(c => c.Carrera)
				.WithMany(c => c.Cursos)
				.HasForeignKey(c => c.CarreraId);

			modelBuilder.Entity<Curso>()
				.HasOne(c => c.Docente)
				.WithMany()
				.HasForeignKey(c => c.DocenteId);

			// ================= MATRICULA =================
			modelBuilder.Entity<Matricula>()
				.HasOne(m => m.Estudiante)
				.WithMany()
				.HasForeignKey(m => m.EstudianteId);

			modelBuilder.Entity<DetalleMatricula>()
				.HasOne(d => d.Matricula)
				.WithMany(m => m.Detalles)
				.HasForeignKey(d => d.MatriculaId);

			modelBuilder.Entity<DetalleMatricula>()
				.HasOne(d => d.Curso)
				.WithMany()
				.HasForeignKey(d => d.CursoId);

			modelBuilder.Entity<Matricula>()
				.Property(m => m.Total)
				.HasPrecision(10, 2);
		}
	}
}