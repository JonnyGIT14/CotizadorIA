using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CotizadorIA.Migrations
{
    /// <inheritdoc />
    public partial class FactorOpcionalProductoTercero : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "FactorAplicado",
                table: "CotizacionesDetalle",
                type: "numeric(8,6)",
                precision: 8,
                scale: 6,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,6)",
                oldPrecision: 8,
                oldScale: 6);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "FactorAplicado",
                table: "CotizacionesDetalle",
                type: "numeric(8,6)",
                precision: 8,
                scale: 6,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,6)",
                oldPrecision: 8,
                oldScale: 6,
                oldNullable: true);
        }
    }
}
