using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CotizadorIA.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDatosFormatoCotizacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CantidadMinimaTexto",
                table: "CotizacionesDetalle",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Clasificacion",
                table: "CotizacionesDetalle",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CropGroup",
                table: "Cotizaciones",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cultivo",
                table: "Cotizaciones",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenciaCliente",
                table: "Cotizaciones",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TipoCambio",
                table: "Cotizaciones",
                type: "numeric(18,6)",
                precision: 18,
                scale: 6,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadMinimaTexto",
                table: "CotizacionesDetalle");

            migrationBuilder.DropColumn(
                name: "Clasificacion",
                table: "CotizacionesDetalle");

            migrationBuilder.DropColumn(
                name: "CropGroup",
                table: "Cotizaciones");

            migrationBuilder.DropColumn(
                name: "Cultivo",
                table: "Cotizaciones");

            migrationBuilder.DropColumn(
                name: "ReferenciaCliente",
                table: "Cotizaciones");

            migrationBuilder.DropColumn(
                name: "TipoCambio",
                table: "Cotizaciones");
        }
    }
}
