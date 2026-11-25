using CotizadorIA.Data;
using CotizadorIA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CotizadorIA.Controllers;

[Authorize(Roles = "Administrador")]
public class ProductoController : Controller
{
    private readonly ProductoData _productoData;
    private readonly ApplicationDbContext _context;

    public ProductoController(ProductoData productoData, ApplicationDbContext context)
    {
        _productoData = productoData;
        _context = context;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var productos = await _productoData.ObtenerProductosAsync(incluirInactivos: true, cancellationToken);
        return View(productos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View("Form", new ProductoFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductoFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        try
        {
            var producto = Mapear(model);
            producto.SubfamiliaProductoId = await ResolverSubfamiliaAsync(model, cancellationToken);
            await _productoData.CrearProductoAsync(producto, cancellationToken);
            TempData["SuccessMessage"] = "Producto creado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(nameof(model.CodigoCatalogo), ex.Message);
            return View("Form", model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var producto = await _productoData.ObtenerProductoAsync(id, cancellationToken);
        return producto is null ? NotFound() : View("Form", Mapear(producto));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductoFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        try
        {
            var producto = Mapear(model);
            producto.SubfamiliaProductoId = await ResolverSubfamiliaAsync(model, cancellationToken);
            var actualizado = await _productoData.ActualizarProductoAsync(id, producto, cancellationToken);
            if (actualizado is null)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Producto actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(nameof(model.CodigoCatalogo), ex.Message);
            return View("Form", model);
        }
    }

    private async Task<int> ResolverSubfamiliaAsync(
        ProductoFormViewModel model,
        CancellationToken cancellationToken)
    {
        var nombreFamilia = model.Familia.Trim();
        var familia = await _context.FamiliasProducto
            .FirstOrDefaultAsync(x => x.Nombre == nombreFamilia, cancellationToken);

        if (familia is null)
        {
            familia = new FamiliaProducto { Nombre = nombreFamilia };
            _context.FamiliasProducto.Add(familia);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var nombreSubfamilia = model.Subfamilia.Trim();
        var subfamilia = await _context.SubfamiliasProducto.FirstOrDefaultAsync(
            x => x.FamiliaProductoId == familia.Id && x.Nombre == nombreSubfamilia,
            cancellationToken);

        if (subfamilia is null)
        {
            subfamilia = new SubfamiliaProducto
            {
                FamiliaProductoId = familia.Id,
                Nombre = nombreSubfamilia
            };
            _context.SubfamiliasProducto.Add(subfamilia);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return subfamilia.Id;
    }

    private static Producto Mapear(ProductoFormViewModel model)
    {
        return new Producto
        {
            Id = model.Id,
            CodigoCatalogo = model.CodigoCatalogo,
            Descripcion = model.Descripcion,
            Unidad = model.Unidad,
            Calibre = model.Calibre,
            Caudal = model.Caudal,
            Espaciamiento = model.Espaciamiento,
            Clasificacion = model.Clasificacion,
            SemanasEntrega = model.SemanasEntrega,
            CantidadMinima = model.CantidadMinima,
            CantidadMinimaTexto = model.CantidadMinimaTexto,
            PrecioLista = model.PrecioLista,
            Activo = model.Activo
        };
    }

    private static ProductoFormViewModel Mapear(Producto producto)
    {
        return new ProductoFormViewModel
        {
            Id = producto.Id,
            CodigoCatalogo = string.IsNullOrWhiteSpace(producto.CodigoCatalogo)
                ? producto.Codigo
                : producto.CodigoCatalogo,
            Descripcion = string.IsNullOrWhiteSpace(producto.Descripcion)
                ? producto.Nombre
                : producto.Descripcion,
            Unidad = producto.Unidad,
            Familia = producto.Subfamilia.Familia.Nombre,
            Subfamilia = producto.Subfamilia.Nombre,
            Calibre = producto.Calibre,
            Caudal = producto.Caudal,
            Espaciamiento = producto.Espaciamiento,
            Clasificacion = producto.Clasificacion,
            SemanasEntrega = producto.SemanasEntrega,
            CantidadMinima = producto.CantidadMinima,
            CantidadMinimaTexto = producto.CantidadMinimaTexto,
            PrecioLista = producto.PrecioLista,
            Activo = producto.Activo
        };
    }
}
