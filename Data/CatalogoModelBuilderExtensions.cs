using Microsoft.EntityFrameworkCore;

namespace CotizadorIA.Data;

public static class CatalogoModelBuilderExtensions
{
    public static ModelBuilder ApplyCatalogoConfigurations(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FamiliaProductoConfiguration());
        modelBuilder.ApplyConfiguration(new SubfamiliaProductoConfiguration());
        modelBuilder.ApplyConfiguration(new ProductoConfiguration());
        modelBuilder.ApplyConfiguration(new ClienteConfiguration());
        modelBuilder.ApplyConfiguration(new CotizacionConfiguration());
        modelBuilder.ApplyConfiguration(new CotizacionDetalleConfiguration());
        return modelBuilder;
    }
}
