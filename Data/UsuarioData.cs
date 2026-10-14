using Microsoft.EntityFrameworkCore;
using CotizadorIA.Models;

namespace CotizadorIA.Data;

public class UsuarioData
{
    public static async Task InicializarAsync(ApplicationDbContext db)
    {
        if (!db.Usuarios.Any())
        {
            var rolAdmin = await db.Rol.FirstOrDefaultAsync(r => r.Nombre == "Administrador");
            if (rolAdmin != null)
            {
                var ahora = DateTime.UtcNow;

                var usuarioAdmin = new Usuario
                {
                    Nombre = "Admin",
                    ApellidoPaterno = "User",
                    ApellidoMaterno = null,
                    Email = "admin@example.com",
                    Password = "password",
                    Activo = true,
                    FechaCreacion = ahora,
                    FechaModificacion = ahora,
                    RolId = rolAdmin.Id
                };
                db.Usuarios.Add(usuarioAdmin);
                await db.SaveChangesAsync();
            }
        }
    }
}