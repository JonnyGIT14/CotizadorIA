using CotizadorIA.Data;
using CotizadorIA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CotizadorIA.Controllers;

[Authorize(Roles = "Administrador")]
public class ClienteController : Controller
{
    private readonly ClienteData _clienteData;

    public ClienteController(ClienteData clienteData)
    {
        _clienteData = clienteData;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var clientes = await _clienteData.ObtenerClientesAsync(incluirInactivos: true, cancellationToken);
        return View(clientes);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View("Form", new ClienteFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClienteFormViewModel model, CancellationToken cancellationToken)
    {
        ValidarFactores(model);
        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        try
        {
            await _clienteData.CrearClienteAsync(Mapear(model), cancellationToken);
            TempData["SuccessMessage"] = "Cliente creado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(nameof(model.CodigoSap), ex.Message);
            return View("Form", model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var cliente = await _clienteData.ObtenerClienteAsync(id, cancellationToken);
        return cliente is null ? NotFound() : View("Form", Mapear(cliente));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ClienteFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        ValidarFactores(model);
        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        try
        {
            var actualizado = await _clienteData.ActualizarClienteAsync(id, Mapear(model), cancellationToken);
            if (actualizado is null)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Cliente actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(nameof(model.CodigoSap), ex.Message);
            return View("Form", model);
        }
    }

    private void ValidarFactores(ClienteFormViewModel model)
    {
        if (model.FactorGeneral is null)
        {
            ModelState.AddModelError(nameof(model.FactorGeneral), "Configura el factor general para poder cotizar a este cliente.");
        }
    }

    private static Cliente Mapear(ClienteFormViewModel model)
    {
        return new Cliente
        {
            Id = model.Id,
            Nombre = model.Nombre,
            Categoria = model.Categoria,
            CodigoSap = model.CodigoSap,
            FactorGeneral = model.FactorGeneral,
            FactorStreamline = model.FactorStreamline,
            Direccion = model.Direccion,
            Email = model.Email,
            Telefono = model.Telefono,
            Apellido = model.Apellido,
            Activo = model.Activo
        };
    }

    private static ClienteFormViewModel Mapear(Cliente cliente)
    {
        return new ClienteFormViewModel
        {
            Id = cliente.Id,
            Nombre = cliente.Nombre,
            Categoria = cliente.Categoria,
            CodigoSap = cliente.CodigoSap,
            FactorGeneral = cliente.FactorGeneral,
            FactorStreamline = cliente.FactorStreamline,
            Direccion = cliente.Direccion,
            Email = cliente.Email,
            Telefono = cliente.Telefono,
            Apellido = cliente.Apellido,
            Activo = cliente.Activo
        };
    }
}
