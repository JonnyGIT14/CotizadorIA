using Microsoft.EntityFrameworkCore;
using CotizadorIA.Models;

namespace CotizadorIA.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Producto> Productos { get; set; }

    public DbSet<Cliente> Clientes { get; set; }

    public DbSet<Rol> Rol { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<FamiliaProducto> FamiliasProducto => Set<FamiliaProducto>();
    public DbSet<SubfamiliaProducto> SubfamiliasProducto => Set<SubfamiliaProducto>();
    public DbSet<Cotizacion> Cotizaciones => Set<Cotizacion>();
    public DbSet<CotizacionDetalle> CotizacionesDetalle => Set<CotizacionDetalle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyCatalogoConfigurations();
    }
}