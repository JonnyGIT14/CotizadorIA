using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CotizadorIA.Migrations
{
    /// <inheritdoc />
    public partial class AgregarBackendCatalogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Productos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Calibre",
                table: "Productos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CantidadMinima",
                table: "Productos",
                type: "numeric(14,3)",
                precision: 14,
                scale: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CantidadMinimaTexto",
                table: "Productos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Caudal",
                table: "Productos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Clasificacion",
                table: "Productos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoCatalogo",
                table: "Productos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Productos",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Espaciamiento",
                table: "Productos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Productos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Productos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioLista",
                table: "Productos",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SemanasEntrega",
                table: "Productos",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubfamiliaProductoId",
                table: "Productos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Unidad",
                table: "Productos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Clientes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Clientes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Clientes",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaveImportacion",
                table: "Clientes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoSap",
                table: "Clientes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FactorGeneral",
                table: "Clientes",
                type: "numeric(8,6)",
                precision: 8,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FactorStreamline",
                table: "Clientes",
                type: "numeric(8,6)",
                precision: 8,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Clientes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Clientes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Cotizaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Folio = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    NombreCliente = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    CodigoSapCliente = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DireccionCliente = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    NombreVendedor = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaVigencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Moneda = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    PorcentajeDescuento = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false),
                    PorcentajeIva = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    MontoDescuento = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    MontoIva = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Total = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cotizaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cotizaciones_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FamiliasProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Activa = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamiliasProducto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CotizacionesDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CotizacionId = table.Column<int>(type: "integer", nullable: false),
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    CodigoCatalogo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DescripcionProducto = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Unidad = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric(14,3)", precision: 14, scale: 3, nullable: false),
                    PrecioListaUnitario = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    FactorAplicado = table.Column<decimal>(type: "numeric(8,6)", precision: 8, scale: 6, nullable: false),
                    PorcentajeDescuento = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false),
                    PrecioNetoUnitario = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    ImporteNeto = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CotizacionesDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CotizacionesDetalle_Cotizaciones_CotizacionId",
                        column: x => x.CotizacionId,
                        principalTable: "Cotizaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CotizacionesDetalle_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubfamiliasProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FamiliaProductoId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Activa = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubfamiliasProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubfamiliasProducto_FamiliasProducto_FamiliaProductoId",
                        column: x => x.FamiliaProductoId,
                        principalTable: "FamiliasProducto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.Sql("""
                INSERT INTO "FamiliasProducto" ("Nombre", "Activa", "FechaCreacion", "FechaModificacion")
                VALUES ('Sin clasificar', TRUE, NOW(), NOW());

                INSERT INTO "SubfamiliasProducto" ("FamiliaProductoId", "Nombre", "Activa", "FechaCreacion", "FechaModificacion")
                SELECT "Id", 'Sin clasificar', TRUE, NOW(), NOW()
                FROM "FamiliasProducto"
                WHERE "Nombre" = 'Sin clasificar';

                UPDATE "Productos"
                SET "CodigoCatalogo" = 'LEGACY-' || "Id"::text,
                    "Descripcion" = LEFT("Nombre", 500),
                    "Unidad" = 'PZA',
                    "SubfamiliaProductoId" = (
                        SELECT sf."Id"
                        FROM "SubfamiliasProducto" sf
                        JOIN "FamiliasProducto" f ON f."Id" = sf."FamiliaProductoId"
                        WHERE f."Nombre" = 'Sin clasificar'
                          AND sf."Nombre" = 'Sin clasificar'
                    ),
                    "Clasificacion" = 'Sin clasificar',
                    "PrecioLista" = "Precio",
                    "Activo" = FALSE,
                    "FechaCreacion" = NOW(),
                    "FechaModificacion" = NOW();

                UPDATE "Clientes"
                SET "ClaveImportacion" = 'LEGACY-' || "Id"::text,
                    "Activo" = FALSE,
                    "FechaCreacion" = NOW(),
                    "FechaModificacion" = NOW();
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CodigoCatalogo",
                table: "Productos",
                column: "CodigoCatalogo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_SubfamiliaProductoId",
                table: "Productos",
                column: "SubfamiliaProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_ClaveImportacion",
                table: "Clientes",
                column: "ClaveImportacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_CodigoSap",
                table: "Clientes",
                column: "CodigoSap",
                unique: true,
                filter: "\"CodigoSap\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_ClienteId",
                table: "Cotizaciones",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_FechaCreacion",
                table: "Cotizaciones",
                column: "FechaCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_CotizacionesDetalle_CotizacionId",
                table: "CotizacionesDetalle",
                column: "CotizacionId");

            migrationBuilder.CreateIndex(
                name: "IX_CotizacionesDetalle_ProductoId",
                table: "CotizacionesDetalle",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_FamiliasProducto_Nombre",
                table: "FamiliasProducto",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubfamiliasProducto_FamiliaProductoId_Nombre",
                table: "SubfamiliasProducto",
                columns: new[] { "FamiliaProductoId", "Nombre" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_SubfamiliasProducto_SubfamiliaProductoId",
                table: "Productos",
                column: "SubfamiliaProductoId",
                principalTable: "SubfamiliasProducto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_SubfamiliasProducto_SubfamiliaProductoId",
                table: "Productos");

            migrationBuilder.DropTable(
                name: "CotizacionesDetalle");

            migrationBuilder.DropTable(
                name: "SubfamiliasProducto");

            migrationBuilder.DropTable(
                name: "Cotizaciones");

            migrationBuilder.DropTable(
                name: "FamiliasProducto");

            migrationBuilder.DropIndex(
                name: "IX_Productos_CodigoCatalogo",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_SubfamiliaProductoId",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_ClaveImportacion",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_CodigoSap",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Calibre",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "CantidadMinima",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "CantidadMinimaTexto",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Caudal",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Clasificacion",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "CodigoCatalogo",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Espaciamiento",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "PrecioLista",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "SemanasEntrega",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "SubfamiliaProductoId",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Unidad",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "ClaveImportacion",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "CodigoSap",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "FactorGeneral",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "FactorStreamline",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Clientes");

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Clientes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
