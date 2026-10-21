using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using CotizadorIA.Data;
using CotizadorIA.Models;

namespace CotizadorIA.Controllers;

public class UsuarioController : Controller
{
    private readonly ApplicationDbContext _db;

    public UsuarioController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadRolesAsync();
        return View(new CreateUsuarioViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUsuarioViewModel model)
    {
        if (ModelState.IsValid)
        {
            var rol = await _db.Rol
                .AsNoTracking()
                .SingleOrDefaultAsync(rol => rol.Id == model.RolId);

            if (rol is null)
            {
                ModelState.AddModelError(nameof(model.RolId), "El rol seleccionado no existe en la base de datos.");
            }
            else if (!rol.Activo)
            {
                ModelState.AddModelError(nameof(model.RolId), "El rol seleccionado está inactivo.");
            }

            if (await _db.Usuarios.AnyAsync(usuario => usuario.Email == model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), "Ya existe un usuario con ese correo.");
            }
        }

        if (!ModelState.IsValid)
        {
            await LoadRolesAsync();
            return View(model);
        }

        var ahora = DateTime.UtcNow;
        var usuario = new Usuario
        {
            Nombre = model.Nombre,
            ApellidoPaterno = model.ApellidoPaterno,
            ApellidoMaterno = model.ApellidoMaterno,
            Email = model.Email,
            Password = model.Password,
            RolId = model.RolId,
            Activo = true,
            FechaCreacion = ahora,
            FechaModificacion = ahora
        };

        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Create));
    }

    private async Task LoadRolesAsync()
    {
        ViewBag.Roles = new SelectList(
            await _db.Rol
                .AsNoTracking()
                .Where(rol => rol.Activo)
                .OrderBy(rol => rol.Nombre)
                .ToListAsync(),
            nameof(Rol.Id),
            nameof(Rol.Nombre));
    }
}