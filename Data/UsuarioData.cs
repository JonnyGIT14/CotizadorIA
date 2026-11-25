using Microsoft.EntityFrameworkCore;
using CotizadorIA.Models;

namespace CotizadorIA.Data;

public class UsuarioData
{
    private readonly ApplicationDbContext _context;
    
    public UsuarioData(ApplicationDbContext context)
    {
        _context = context;
    }

    //Create un nuevo usuario a la base de datos
    public  async Task<Usuario> CreateUsuario( CreateUsuarioViewModel usuario)
    {
       var User = new Usuario
        {
            Nombre = usuario.Nombre,
            ApellidoPaterno = usuario.ApellidoPaterno,
            ApellidoMaterno = usuario.ApellidoMaterno,
            Email = usuario.Email,
            Password = usuario.Password,
            RolId = usuario.RolId,
            Activo = true,
            FechaCreacion = DateTime.UtcNow,
            FechaModificacion = DateTime.UtcNow
        };

        _context.Usuarios.Add(User);
        await _context.SaveChangesAsync();
        return User;
    }

    public async Task<List<Usuario>> ObtenerUsuarios()
    {
        return await _context.Usuarios.ToListAsync();
    }

}