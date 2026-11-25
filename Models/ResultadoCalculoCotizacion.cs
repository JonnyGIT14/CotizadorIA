namespace CotizadorIA.Models;

public class ResultadoCalculoCotizacion
{
    public decimal Subtotal { get; set; }
    public decimal PorcentajeDescuento { get; set; }
    public decimal MontoDescuento { get; set; }
    public decimal Neto { get; set; }
    public decimal PorcentajeIva { get; set; }
    public decimal MontoIva { get; set; }
    public decimal Total { get; set; }
    public List<LineaCotizacionCalculada> Lineas { get; set; } = new();
    public List<string> Avisos { get; set; } = new();
}

public class LineaCotizacionCalculada
{
    public Producto? Producto { get; set; }
    public string CodigoCatalogo { get; set; } = string.Empty;
    public string DescripcionProducto { get; set; } = string.Empty;
    public string Unidad { get; set; } = string.Empty;
    public decimal PrecioListaUnitario { get; set; }
    public bool EsProductoTercero { get; set; }
    public decimal Cantidad { get; set; }
    public decimal? FactorAplicado { get; set; }
    public decimal PrecioNetoUnitario { get; set; }
    public decimal PorcentajeDescuento { get; set; }
    public decimal ImporteNeto { get; set; }
}

public class SolicitudLineaCotizacion
{
    public Producto? Producto { get; set; }
    public string CodigoCatalogo { get; set; } = string.Empty;
    public string DescripcionProducto { get; set; } = string.Empty;
    public string Unidad { get; set; } = string.Empty;
    public decimal PrecioListaUnitario { get; set; }
    public decimal Cantidad { get; set; }
    public decimal? SemanasEntrega { get; set; }
    public decimal? CantidadMinima { get; set; }
    public string? CantidadMinimaTexto { get; set; }
    public bool EsProductoTercero { get; set; }
}
