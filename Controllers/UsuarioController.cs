using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using CotizadorIA.Data;
using CotizadorIA.Models;
using Microsoft.AspNetCore.Authorization;

namespace CotizadorIA.Controllers;

[Authorize(Roles = "Administrador")]
public class UsuarioController : Controller
{
    private readonly UsuarioData _usuarioData;
    private readonly RolData _rolData;

    public UsuarioController(RolData rolData, UsuarioData usuarioData)
    {
        _rolData = rolData;
        _usuarioData = usuarioData;
    }

    private async Task CargarRoles()
    {
        var roles = await _rolData.ObtenerRoles();
        ViewBag.Roles = new SelectList(roles, "Id", "Nombre");
    }

    // GET: Usuario/Create
    public async Task<IActionResult> CreateUser()
    {
        await CargarRoles();

        return View(new CreateUsuarioViewModel());
    }

    // POST: Usuario/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(CreateUsuarioViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await CargarRoles();
            return View(model);
        }

        await _usuarioData.CreateUsuario(model);
        TempData["SuccessMessage"] = "Usuario creado exitosamente.";
        return RedirectToAction(nameof(CreateUser));
    }

}