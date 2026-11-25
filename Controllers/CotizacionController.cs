using System.Security.Cryptography;
using System.Text.Json;
using CotizadorIA.Data;
using CotizadorIA.Models;
using CotizadorIA.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Security.Claims;

namespace CotizadorIA.Controllers;

[Authorize(Roles = "Administrador")]
public class CotizacionController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly CotizacionCalculator _calculator;
    private readonly FrankfurterExchangeRateService _exchangeRateService;
    private readonly ILogger<CotizacionController> _logger;

    public CotizacionController(
        ApplicationDbContext context,
        CotizacionCalculator calculator,
        FrankfurterExchangeRateService exchangeRateService,
        ILogger<CotizacionController> logger)
    {
        _context = context;
        _calculator = calculator;
        _exchangeRateService = exchangeRateService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var cotizaciones = await _context.Cotizaciones
            .AsNoTracking()
            .OrderByDescending(x => x.FechaCreacion)
            .ToListAsync(cancellationToken);
        return View(cotizaciones);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var model = new CotizacionFormViewModel();
        try
        {
            var tipoCambio = await _exchangeRateService.GetLatestUsdToMxnAsync(cancellationToken);
            model.TipoCambio = tipoCambio.Rate;
        }
        catch (HttpRequestException ex)
        {
            RegistrarFalloTipoCambio(ex);
        }
        catch (JsonException ex)
        {
            RegistrarFalloTipoCambio(ex);
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            RegistrarFalloTipoCambio(ex);
        }

        await CargarOpcionesAsync(cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CotizacionFormViewModel model,
        CancellationToken cancellationToken)
    {
        var claimUsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(claimUsuarioId, out var usuarioId))
        {
            return Challenge();
        }

        if (model.Lineas.Count == 0)
        {
            ModelState.AddModelError(nameof(model.Lineas), "Agrega al menos un producto.");
        }

        if (model.FechaCotizacion < new DateTime(2000, 1, 1)
            || model.FechaCotizacion > new DateTime(2100, 12, 31))
        {
            ModelState.AddModelError(nameof(model.FechaCotizacion), "La fecha de cotización no es válida.");
        }

        for (var index = 0; index < model.Lineas.Count; index++)
        {
            var linea = model.Lineas[index];
            if (linea.EsTercero)
            {
                if (string.IsNullOrWhiteSpace(linea.CodigoCatalogo)
                    || linea.CodigoCatalogo.Trim().Length > 50)
                {
                    ModelState.AddModelError(
                        $"Lineas[{index}].CodigoCatalogo",
                        "El SKU del producto de terceros es obligatorio y no debe exceder 50 caracteres.");
                }

                if (string.IsNullOrWhiteSpace(linea.DescripcionProducto)
                    || linea.DescripcionProducto.Trim().Length > 500)
                {
                    ModelState.AddModelError(
                        $"Lineas[{index}].DescripcionProducto",
                        "La descripción es obligatoria y no debe exceder 500 caracteres.");
                }

                if (string.IsNullOrWhiteSpace(linea.Unidad) || linea.Unidad.Trim().Length > 20)
                {
                    ModelState.AddModelError(
                        $"Lineas[{index}].Unidad",
                        "La unidad es obligatoria y no debe exceder 20 caracteres.");
                }

                if (linea.PrecioBase is null
                    || linea.PrecioBase < 0
                    || linea.PrecioBase > 999999999999m)
                {
                    ModelState.AddModelError(
                        $"Lineas[{index}].PrecioBase",
                        "El costo unitario debe ser un valor válido mayor o igual a cero.");
                }
            }
            else if (linea.ProductoId is null or <= 0)
            {
                ModelState.AddModelError(
                    $"Lineas[{index}].ProductoId",
                    "Selecciona un producto del catálogo.");
            }
        }

        if (!ModelState.IsValid)
        {
            await CargarOpcionesAsync(cancellationToken);
            return View(model);
        }

        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(x => x.Id == model.ClienteId && x.Activo, cancellationToken);
        if (cliente is null)
        {
            ModelState.AddModelError(nameof(model.ClienteId), "El cliente seleccionado no está disponible.");
            await CargarOpcionesAsync(cancellationToken);
            return View(model);
        }

        var usuario = await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == usuarioId && x.Activo, cancellationToken);
        if (usuario is null)
        {
            await HttpContext.SignOutAsync();
            return Challenge();
        }

        var idsProductoCatalogo = model.Lineas
            .Where(x => !x.EsTercero)
            .Select(x => x.ProductoId!.Value)
            .Distinct()
            .ToList();

        var productos = await _context.Productos
            .Include(x => x.Subfamilia)
            .ThenInclude(x => x.Familia)
            .Where(x => idsProductoCatalogo.Contains(x.Id)
                && x.Activo
                && x.Subfamilia.Activa
                && x.Subfamilia.Familia.Activa)
            .ToListAsync(cancellationToken);

        if (productos.Count != idsProductoCatalogo.Count)
        {
            ModelState.AddModelError(nameof(model.Lineas), "Uno o más productos ya no están disponibles.");
            await CargarOpcionesAsync(cancellationToken);
            return View(model);
        }

        var productosPorId = productos.ToDictionary(x => x.Id);
        var solicitudes = new List<SolicitudLineaCotizacion>();
        foreach (var linea in model.Lineas)
        {
            if (linea.EsTercero)
            {
                solicitudes.Add(new SolicitudLineaCotizacion
                {
                    CodigoCatalogo = linea.CodigoCatalogo.Trim(),
                    DescripcionProducto = linea.DescripcionProducto.Trim(),
                    Unidad = linea.Unidad.Trim(),
                    PrecioListaUnitario = linea.PrecioBase!.Value,
                    Cantidad = linea.Cantidad,
                    EsProductoTercero = true
                });
            }
            else
            {
                var producto = productosPorId[linea.ProductoId!.Value];
                solicitudes.Add(new SolicitudLineaCotizacion
                {
                    Producto = producto,
                    CodigoCatalogo = producto.CodigoCatalogo,
                    DescripcionProducto = producto.Descripcion,
                    Unidad = producto.Unidad,
                    PrecioListaUnitario = producto.PrecioLista,
                    Cantidad = linea.Cantidad,
                    SemanasEntrega = producto.SemanasEntrega,
                    CantidadMinima = producto.CantidadMinima,
                    CantidadMinimaTexto = producto.CantidadMinimaTexto
                });
            }
        }

        ResultadoCalculoCotizacion calculo;
        try
        {
            calculo = _calculator.Calcular(
                cliente,
                solicitudes,
                model.AplicarIva,
                model.PorcentajeDescuento,
                model.ProyectoGrowSphere);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await CargarOpcionesAsync(cancellationToken);
            return View(model);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await CargarOpcionesAsync(cancellationToken);
            return View(model);
        }

        var cotizacion = new Cotizacion
        {
            Folio = $"COT-{DateTime.UtcNow:yyyyMMdd}-{Convert.ToHexString(RandomNumberGenerator.GetBytes(4))}",
            ClienteId = cliente.Id,
            UsuarioId = usuario.Id,
            FechaCotizacion = DateTime.SpecifyKind(model.FechaCotizacion.Date, DateTimeKind.Unspecified),
            NombreCliente = cliente.Nombre,
            CodigoSapCliente = cliente.CodigoSap,
            ReferenciaCliente = cliente.ClaveImportacion,
            DireccionCliente = cliente.Direccion,
            NombreVendedor = string.Join(
                " ",
                new[] { usuario.Nombre, usuario.ApellidoPaterno, usuario.ApellidoMaterno }
                    .Where(nombre => !string.IsNullOrWhiteSpace(nombre))),
            FechaCreacion = DateTime.UtcNow,
            FechaVigencia = model.FechaVigencia.HasValue
                ? DateTime.SpecifyKind(model.FechaVigencia.Value.Date, DateTimeKind.Local).ToUniversalTime()
                : null,
            Moneda = "USD",
            TipoCambio = model.TipoCambio,
            Cultivo = model.Cultivo.Trim(),
            CropGroup = model.CropGroup.Trim(),
            PorcentajeDescuento = calculo.PorcentajeDescuento,
            PorcentajeIva = calculo.PorcentajeIva,
            Subtotal = calculo.Subtotal,
            MontoDescuento = calculo.MontoDescuento,
            MontoIva = calculo.MontoIva,
            Total = calculo.Total,
            Estado = "Borrador"
        };

        foreach (var linea in calculo.Lineas)
        {
            cotizacion.Detalles.Add(new CotizacionDetalle
            {
                ProductoId = linea.Producto?.Id,
                EsProductoTercero = linea.EsProductoTercero,
                CodigoCatalogo = linea.CodigoCatalogo,
                DescripcionProducto = linea.DescripcionProducto,
                Clasificacion = linea.Producto?.Clasificacion
                    ?? (linea.EsProductoTercero ? "Producto de terceros" : null),
                Unidad = linea.Unidad,
                CantidadMinimaTexto = linea.Producto?.CantidadMinimaTexto
                    ?? linea.Producto?.CantidadMinima?.ToString("N3"),
                Cantidad = linea.Cantidad,
                PrecioListaUnitario = linea.PrecioListaUnitario,
                FactorAplicado = linea.FactorAplicado,
                PorcentajeDescuento = linea.PorcentajeDescuento,
                PrecioNetoUnitario = linea.PrecioNetoUnitario
                    * (1m - linea.PorcentajeDescuento / 100m),
                ImporteNeto = linea.ImporteNeto
            });
        }

        _context.Cotizaciones.Add(cotizacion);
        await _context.SaveChangesAsync(cancellationToken);
        TempData["SuccessMessage"] = $"Cotización {cotizacion.Folio} creada correctamente.";
        TempData["QuoteWarnings"] = JsonSerializer.Serialize(calculo.Avisos);
        return RedirectToAction(nameof(Details), new { id = cotizacion.Id });
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var cotizacion = await _context.Cotizaciones
            .AsNoTracking()
            .Include(x => x.Detalles)
            .Include(x => x.Usuario)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (TempData["QuoteWarnings"] is string avisosJson)
        {
            ViewBag.Avisos = JsonSerializer.Deserialize<List<string>>(avisosJson) ?? new List<string>();
        }

        return cotizacion is null ? NotFound() : View(cotizacion);
    }

    private async Task CargarOpcionesAsync(CancellationToken cancellationToken)
    {
        ViewBag.Clientes = await _context.Clientes
            .AsNoTracking()
            .Where(x => x.Activo)
            .OrderBy(x => x.Nombre)
            .ToListAsync(cancellationToken);

        ViewBag.Productos = await _context.Productos
            .AsNoTracking()
            .Include(x => x.Subfamilia)
            .ThenInclude(x => x.Familia)
            .Where(x => x.Activo && x.Subfamilia.Activa && x.Subfamilia.Familia.Activa)
            .OrderBy(x => x.CodigoCatalogo)
            .ToListAsync(cancellationToken);
    }

    private void RegistrarFalloTipoCambio(Exception exception)
    {
        _logger.LogWarning(exception, "No fue posible consultar el tipo de cambio USD/MXN.");
        ViewBag.TipoCambioWarning =
            "No se pudo consultar el tipo de cambio automáticamente. Captúralo manualmente para continuar.";
    }
}
