using CotizadorIA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CotizadorIA.Data;

public class FamiliaProductoConfiguration : IEntityTypeConfiguration<FamiliaProducto>
{
    public void Configure(EntityTypeBuilder<FamiliaProducto> builder)
    {
        builder.ToTable("FamiliasProducto");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Nombre).HasMaxLength(120).IsRequired();
        builder.HasIndex(x => x.Nombre).IsUnique();
    }
}

public class SubfamiliaProductoConfiguration : IEntityTypeConfiguration<SubfamiliaProducto>
{
    public void Configure(EntityTypeBuilder<SubfamiliaProducto> builder)
    {
        builder.ToTable("SubfamiliasProducto");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Nombre).HasMaxLength(160).IsRequired();
        builder.HasIndex(x => new { x.FamiliaProductoId, x.Nombre }).IsUnique();
        builder.HasOne(x => x.Familia)
            .WithMany(x => x.Subfamilias)
            .HasForeignKey(x => x.FamiliaProductoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("Productos");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CodigoCatalogo).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Descripcion).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Unidad).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Calibre).HasMaxLength(100);
        builder.Property(x => x.Caudal).HasMaxLength(100);
        builder.Property(x => x.Espaciamiento).HasMaxLength(100);
        builder.Property(x => x.Clasificacion).HasMaxLength(100).IsRequired();
        builder.Property(x => x.CantidadMinimaTexto).HasMaxLength(100);
        builder.Property(x => x.SemanasEntrega).HasPrecision(10, 2);
        builder.Property(x => x.CantidadMinima).HasPrecision(14, 3);
        builder.Property(x => x.PrecioLista).HasPrecision(18, 4);
        builder.HasIndex(x => x.CodigoCatalogo).IsUnique();
        builder.HasIndex(x => x.SubfamiliaProductoId);
        builder.HasOne(x => x.Subfamilia)
            .WithMany(x => x.Productos)
            .HasForeignKey(x => x.SubfamiliaProductoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ClaveImportacion).HasMaxLength(100).IsRequired();
        builder.Property(x => x.CodigoSap).HasMaxLength(50);
        builder.Property(x => x.Categoria).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.Nombre).IsRequired();
        builder.Property(x => x.Direccion);
        builder.Property(x => x.FactorGeneral).HasPrecision(8, 6);
        builder.Property(x => x.FactorStreamline).HasPrecision(8, 6);
        builder.HasIndex(x => x.ClaveImportacion).IsUnique();
        builder.HasIndex(x => x.CodigoSap)
            .IsUnique()
            .HasFilter("\"CodigoSap\" IS NOT NULL");
    }
}

public class CotizacionConfiguration : IEntityTypeConfiguration<Cotizacion>
{
    public void Configure(EntityTypeBuilder<Cotizacion> builder)
    {
        builder.ToTable("Cotizaciones");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Folio).HasMaxLength(40).IsRequired();
        builder.Property(x => x.NombreCliente).HasMaxLength(250).IsRequired();
        builder.Property(x => x.CodigoSapCliente).HasMaxLength(50);
        builder.Property(x => x.ReferenciaCliente).HasMaxLength(100);
        builder.Property(x => x.DireccionCliente).HasMaxLength(1000);
        builder.Property(x => x.NombreVendedor).HasMaxLength(250).IsRequired();
        builder.Property(x => x.Moneda).HasMaxLength(3).IsRequired();
        builder.Property(x => x.TipoCambio).HasPrecision(18, 6);
        builder.Property(x => x.Cultivo).HasMaxLength(100);
        builder.Property(x => x.CropGroup).HasMaxLength(100);
        builder.Property(x => x.Estado).HasMaxLength(30).IsRequired();
        builder.Property(x => x.FechaCotizacion).HasColumnType("date");
        builder.Property(x => x.PorcentajeDescuento).HasPrecision(8, 4);
        builder.Property(x => x.PorcentajeIva).HasPrecision(8, 4);
        builder.Property(x => x.Subtotal).HasPrecision(18, 4);
        builder.Property(x => x.MontoDescuento).HasPrecision(18, 4);
        builder.Property(x => x.MontoIva).HasPrecision(18, 4);
        builder.Property(x => x.Total).HasPrecision(18, 4);
        builder.HasIndex(x => x.ClienteId);
        builder.HasIndex(x => x.FechaCreacion);
        builder.HasIndex(x => x.UsuarioId);
        builder.HasOne(x => x.Cliente)
            .WithMany()
            .HasForeignKey(x => x.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class CotizacionDetalleConfiguration : IEntityTypeConfiguration<CotizacionDetalle>
{
    public void Configure(EntityTypeBuilder<CotizacionDetalle> builder)
    {
        builder.ToTable("CotizacionesDetalle");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CodigoCatalogo).HasMaxLength(50).IsRequired();
        builder.Property(x => x.DescripcionProducto).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Clasificacion).HasMaxLength(100);
        builder.Property(x => x.Unidad).HasMaxLength(20).IsRequired();
        builder.Property(x => x.CantidadMinimaTexto).HasMaxLength(100);
        builder.Property(x => x.Cantidad).HasPrecision(14, 3);
        builder.Property(x => x.PrecioListaUnitario).HasPrecision(18, 4);
        builder.Property(x => x.FactorAplicado).HasPrecision(8, 6);
        builder.Property(x => x.PorcentajeDescuento).HasPrecision(8, 4);
        builder.Property(x => x.PrecioNetoUnitario).HasPrecision(18, 4);
        builder.Property(x => x.ImporteNeto).HasPrecision(18, 4);
        builder.HasOne(x => x.Cotizacion)
            .WithMany(x => x.Detalles)
            .HasForeignKey(x => x.CotizacionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.EsProductoTercero).IsRequired();
        builder.HasOne(x => x.Producto)
            .WithMany()
            .HasForeignKey(x => x.ProductoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
