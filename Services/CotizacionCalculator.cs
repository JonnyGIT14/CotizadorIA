using System.Globalization;
using System.Text;
using CotizadorIA.Models;

namespace CotizadorIA.Services;

public class CotizacionCalculator
{
    public ResultadoCalculoCotizacion Calcular(
        Cliente cliente,
        IEnumerable<(Producto Producto, decimal Cantidad)> productos,
        bool aplicarIva,
        decimal porcentajeDescuento,
        bool proyectoGrowSphere = false)
    {
        ArgumentNullException.ThrowIfNull(productos);
        var lineas = productos.ToList();
        if (lineas.Any(x => x.Producto is null))
        {
            throw new ArgumentException("La lista contiene un producto nulo.", nameof(productos));
        }

        return Calcular(
            cliente,
            lineas.Select(x => new SolicitudLineaCotizacion
            {
                Producto = x.Producto,
                CodigoCatalogo = x.Producto.CodigoCatalogo,
                DescripcionProducto = x.Producto.Descripcion,
                Unidad = x.Producto.Unidad,
                PrecioListaUnitario = x.Producto.PrecioLista,
                Cantidad = x.Cantidad,
                SemanasEntrega = x.Producto.SemanasEntrega,
                CantidadMinima = x.Producto.CantidadMinima,
                CantidadMinimaTexto = x.Producto.CantidadMinimaTexto
            }),
            aplicarIva,
            porcentajeDescuento,
            proyectoGrowSphere);
    }

    public ResultadoCalculoCotizacion Calcular(
        Cliente cliente,
        IEnumerable<SolicitudLineaCotizacion> productos,
        bool aplicarIva,
        decimal porcentajeDescuento,
        bool proyectoGrowSphere = false)
    {
        ArgumentNullException.ThrowIfNull(cliente);
        ArgumentNullException.ThrowIfNull(productos);

        if (porcentajeDescuento is < 0 or > 20)
        {
            throw new ArgumentOutOfRangeException(
                nameof(porcentajeDescuento),
                "El descuento debe estar entre 0 y 20 por ciento.");
        }

        var solicitudes = productos.ToList();
        if (solicitudes.Any(x => x.Producto is null && !x.EsProductoTercero))
        {
            throw new ArgumentException("La lista contiene una línea de producto inválida.", nameof(productos));
        }

        if (solicitudes.Any(x => x.Cantidad <= 0))
        {
            throw new ArgumentException("La cantidad de cada producto debe ser mayor que cero.", nameof(productos));
        }

        var streamlinePorLinea = solicitudes.Select(x => EsStreamline(x.Producto)).ToList();
        var carritoSoloStreamline = streamlinePorLinea.Count > 0 && streamlinePorLinea.All(x => x);
        var descuentoElegible = cliente.FactorGeneral == 0.55m
            && solicitudes.Select((linea, index) => (linea, streamlinePorLinea[index]))
                .Any(x => !x.linea.EsProductoTercero && !x.Item2);
        var descuentoAplicado = descuentoElegible ? porcentajeDescuento : 0m;
        var ivaAplicado = aplicarIva && solicitudes.Count > 0 && !carritoSoloStreamline;

        var resultado = new ResultadoCalculoCotizacion
        {
            PorcentajeDescuento = descuentoAplicado,
            PorcentajeIva = ivaAplicado ? 16m : 0m
        };

        decimal baseDescuento = 0m;
        foreach (var solicitud in solicitudes)
        {
            var streamline = EsStreamline(solicitud.Producto);
            decimal? factor = solicitud.EsProductoTercero
                ? null
                : streamline
                    ? cliente.FactorStreamline
                        ?? throw new InvalidOperationException(
                            $"El cliente '{cliente.Nombre}' no tiene factor Streamline configurado.")
                    : cliente.FactorGeneral
                        ?? throw new InvalidOperationException(
                            $"El cliente '{cliente.Nombre}' no tiene factor general configurado.");
            var precioNetoUnitario = solicitud.PrecioListaUnitario * (factor ?? 1m);
            var importeAntesDescuento = precioNetoUnitario * solicitud.Cantidad;
            var descuentoLinea = !solicitud.EsProductoTercero && !streamline && descuentoElegible
                ? descuentoAplicado
                : 0m;
            var importeNeto = importeAntesDescuento * (1m - descuentoLinea / 100m);

            resultado.Subtotal += importeAntesDescuento;
            if (!solicitud.EsProductoTercero && !streamline)
            {
                baseDescuento += importeAntesDescuento;
            }

            resultado.Lineas.Add(new LineaCotizacionCalculada
            {
                Producto = solicitud.Producto,
                CodigoCatalogo = solicitud.CodigoCatalogo,
                DescripcionProducto = solicitud.DescripcionProducto,
                Unidad = solicitud.Unidad,
                PrecioListaUnitario = solicitud.PrecioListaUnitario,
                EsProductoTercero = solicitud.EsProductoTercero,
                Cantidad = solicitud.Cantidad,
                FactorAplicado = factor,
                PrecioNetoUnitario = precioNetoUnitario,
                PorcentajeDescuento = descuentoLinea,
                ImporteNeto = importeNeto
            });
        }

        resultado.MontoDescuento = baseDescuento * (descuentoAplicado / 100m);
        resultado.Neto = resultado.Subtotal - resultado.MontoDescuento;
        resultado.MontoIva = ivaAplicado ? resultado.Neto * 0.16m : 0m;
        resultado.Total = resultado.Neto + resultado.MontoIva;
        resultado.Avisos = CrearAvisos(cliente, solicitudes, proyectoGrowSphere);
        return resultado;
    }

    private static List<string> CrearAvisos(
        Cliente cliente,
        IEnumerable<SolicitudLineaCotizacion> productos,
        bool proyectoGrowSphere)
    {
        var avisos = new List<string>();
        foreach (var solicitud in productos)
        {
            if (solicitud.EsProductoTercero)
            {
                continue;
            }

            var streamline = EsStreamline(solicitud.Producto);
            var factor = streamline ? cliente.FactorStreamline : cliente.FactorGeneral;
            if (!streamline && factor is < 0.35m)
            {
                avisos.Add($"Margen bajo en {solicitud.CodigoCatalogo} (factor {factor:0.000}).");
            }

            if (solicitud.SemanasEntrega > 4)
            {
                avisos.Add(
                    $"Entrega estimada de {solicitud.SemanasEntrega:0.##} semanas: {solicitud.CodigoCatalogo}.");
            }

            if (solicitud.CantidadMinima is not null
                && solicitud.Cantidad < solicitud.CantidadMinima.Value)
            {
                var cantidadTexto = solicitud.CantidadMinimaTexto
                    ?? solicitud.CantidadMinima.Value.ToString(CultureInfo.InvariantCulture);
                avisos.Add(
                    $"Cantidad menor al mínimo de compra ({cantidadTexto} {solicitud.Unidad}) en {solicitud.CodigoCatalogo}.");
            }
        }

        if (proyectoGrowSphere)
        {
            avisos.Add("Proyecto con posible interés en GrowSphere; mencionar en seguimiento.");
        }

        return avisos;
    }

    private static bool EsStreamline(Producto? producto)
    {
        if (producto is null)
        {
            return false;
        }

        var nombreFamilia = producto.Subfamilia?.Familia?.Nombre ?? string.Empty;
        var normalizado = nombreFamilia.Normalize(NormalizationForm.FormD);
        var sinAcentos = new string(normalizado
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .ToArray());
        return sinAcentos.Contains("streamline", StringComparison.OrdinalIgnoreCase);
    }
}
