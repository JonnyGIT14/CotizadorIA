namespace CotizadorIA.Models;

public class Producto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string CodigoCatalogo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Unidad { get; set; } = string.Empty;
    public int SubfamiliaProductoId { get; set; }
    public string? Calibre { get; set; }
    public string? Caudal { get; set; }
    public string? Espaciamiento { get; set; }
    public string Clasificacion { get; set; } = string.Empty;
    public decimal? SemanasEntrega { get; set; }
    public decimal? CantidadMinima { get; set; }
    public string? CantidadMinimaTexto { get; set; }
    public decimal PrecioLista { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;

    public SubfamiliaProducto Subfamilia { get; set; } = null!;
}
