namespace CotizadorIA.Models;

public class CotizacionDetalle
{
    public int Id { get; set; }
    public int CotizacionId { get; set; }
    public int? ProductoId { get; set; }
    public bool EsProductoTercero { get; set; }
    public string CodigoCatalogo { get; set; } = string.Empty;
    public string DescripcionProducto { get; set; } = string.Empty;
    public string? Clasificacion { get; set; }
    public string Unidad { get; set; } = string.Empty;
    public string? CantidadMinimaTexto { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioListaUnitario { get; set; }
    public decimal? FactorAplicado { get; set; }
    public decimal PorcentajeDescuento { get; set; }
    public decimal PrecioNetoUnitario { get; set; }
    public decimal ImporteNeto { get; set; }

    public Cotizacion Cotizacion { get; set; } = null!;
    public Producto Producto { get; set; } = null!;
}
