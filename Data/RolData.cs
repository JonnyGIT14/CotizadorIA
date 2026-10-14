using Microsoft.EntityFrameworkCore;
using CotizadorIA.Models;


namespace CotizadorIA.Data;

public class RolData
{

    public static async Task InicializarAsync(ApplicationDbContext db)
    {
        if (!await db.Rol.AnyAsync())
        {
            var ahora = DateTime.UtcNow;

            db.Rol.AddRange(new Rol
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

            await db.SaveChangesAsync();
        }
    }

}