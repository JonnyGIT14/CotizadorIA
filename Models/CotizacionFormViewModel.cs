using System.ComponentModel.DataAnnotations;

namespace CotizadorIA.Models;

public class CotizacionFormViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un cliente.")]
    public int ClienteId { get; set; }

    [DataType(DataType.Date)]
    public DateTime FechaCotizacion { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    public DateTime? FechaVigencia { get; set; }

    [Required(ErrorMessage = "Captura el tipo de cambio.")]
    [Range(typeof(decimal), "0.000001", "999999999", ErrorMessage = "El tipo de cambio debe ser mayor que cero.")]
    public decimal? TipoCambio { get; set; }

    [Required(ErrorMessage = "Captura el cultivo.")]
    [StringLength(100)]
    public string Cultivo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Captura el crop group.")]
    [StringLength(100)]
    public string CropGroup { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "20")]
    public decimal PorcentajeDescuento { get; set; }

    public bool AplicarIva { get; set; }

    public bool ProyectoGrowSphere { get; set; }

    public List<CotizacionLineaFormViewModel> Lineas { get; set; } = new();
}

public class CotizacionLineaFormViewModel
{
    public int? ProductoId { get; set; }

    public bool EsTercero { get; set; }

    public string CodigoCatalogo { get; set; } = string.Empty;

    public string DescripcionProducto { get; set; } = string.Empty;

    public string Unidad { get; set; } = string.Empty;

    public decimal? PrecioBase { get; set; }

    [Range(typeof(decimal), "0.001", "999999999", ErrorMessage = "La cantidad debe ser mayor que cero.")]
    public decimal Cantidad { get; set; } = 1;
}
