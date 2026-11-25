using Microsoft.EntityFrameworkCore;
using CotizadorIA.Models;


namespace CotizadorIA.Data;

public class RolData
{

    private readonly ApplicationDbContext _context;

    public RolData(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Rol>> ObtenerRoles()
    {
        return await _context.Rol.ToListAsync();
    }

    public  async Task InicializarAsync()
    {
        if (!await _context.Rol.AnyAsync())
        {
            var ahora = DateTime.UtcNow;

            _context.Rol.AddRange(new Rol
            {
                Nombre = "Administrador",
                Descripcion = "Acceso completo al sistema",
                Activo = true,
                FechaCreacion = ahora,
                FechaModificacion = ahora
            },
            new Rol
            {
                Nombre = "Analista de Precios",
                Descripcion = "Gestion de costos y precios",
                Activo = true,
                FechaCreacion = ahora,
                FechaModificacion = ahora
            },
            new Rol
            {
                Nombre = "Ejecutivo Comercial",
                Descripcion = "Generacion de cotizaciones",
                Activo = true,
                FechaCreacion = ahora,
                FechaModificacion = ahora
            },
            new Rol
            {
                Nombre = "Gerente comercial",
                Descripcion = "Supervicion Comercial",
                Activo = true,
                FechaCreacion = ahora,
                FechaModificacion = ahora
            }
            );

            await _context.SaveChangesAsync();
        }
    }

   
}