using System.ComponentModel.DataAnnotations;

namespace CotizadorIA.Models;

public class ProductoFormViewModel
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string CodigoCatalogo { get; set; } = string.Empty;

    [Required, StringLength(500)]
    public string Descripcion { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Unidad { get; set; } = "PZA";

    [Required, StringLength(120)]
    public string Familia { get; set; } = string.Empty;

    [Required, StringLength(160)]
    public string Subfamilia { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Calibre { get; set; }

    [StringLength(100)]
    public string? Caudal { get; set; }

    [StringLength(100)]
    public string? Espaciamiento { get; set; }

    [Required, StringLength(100)]
    public string Clasificacion { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "10000")]
    public decimal? SemanasEntrega { get; set; }

    [Range(typeof(decimal), "0", "100000000")]
    public decimal? CantidadMinima { get; set; }

    [StringLength(100)]
    public string? CantidadMinimaTexto { get; set; }

    [Range(typeof(decimal), "0", "999999999999")]
    public decimal PrecioLista { get; set; }

    public bool Activo { get; set; } = true;
}
