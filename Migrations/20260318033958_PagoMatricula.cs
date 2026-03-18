using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal_VillarrealHugo.Migrations
{
    /// <inheritdoc />
    public partial class PagoMatricula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comprobante",
                table: "Matriculas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoPago",
                table: "Matriculas",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comprobante",
                table: "Matriculas");

            migrationBuilder.DropColumn(
                name: "EstadoPago",
                table: "Matriculas");
        }
    }
}
