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
}