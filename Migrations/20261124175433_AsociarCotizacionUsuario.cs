using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CotizadorIA.Migrations
{
    /// <inheritdoc />
    public partial class AsociarCotizacionUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Cotizaciones",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_UsuarioId",
                table: "Cotizaciones",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cotizaciones_Usuarios_UsuarioId",
                table: "Cotizaciones",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cotizaciones_Usuarios_UsuarioId",
                table: "Cotizaciones");

            migrationBuilder.DropIndex(
                name: "IX_Cotizaciones_UsuarioId",
                table: "Cotizaciones");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Cotizaciones");
        }
    }
}
