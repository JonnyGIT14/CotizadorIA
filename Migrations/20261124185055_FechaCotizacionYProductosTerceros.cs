using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CotizadorIA.Migrations
{
    /// <inheritdoc />
    public partial class FechaCotizacionYProductosTerceros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CotizacionesDetalle_Productos_ProductoId",
                table: "CotizacionesDetalle");

            migrationBuilder.AlterColumn<int>(
                name: "ProductoId",
                table: "CotizacionesDetalle",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<bool>(
                name: "EsProductoTercero",
                table: "CotizacionesDetalle",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCotizacion",
                table: "Cotizaciones",
                type: "date",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE "Cotizaciones"
                SET "FechaCotizacion" = "FechaCreacion"::date;
                """);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCotizacion",
                table: "Cotizaciones",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CotizacionesDetalle_Productos_ProductoId",
                table: "CotizacionesDetalle",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CotizacionesDetalle_Productos_ProductoId",
                table: "CotizacionesDetalle");

            migrationBuilder.DropColumn(
                name: "EsProductoTercero",
                table: "CotizacionesDetalle");

            migrationBuilder.DropColumn(
                name: "FechaCotizacion",
                table: "Cotizaciones");

            migrationBuilder.AlterColumn<int>(
                name: "ProductoId",
                table: "CotizacionesDetalle",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CotizacionesDetalle_Productos_ProductoId",
                table: "CotizacionesDetalle",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
