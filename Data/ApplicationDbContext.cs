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
}