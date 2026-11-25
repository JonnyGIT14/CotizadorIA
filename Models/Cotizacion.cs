namespace CotizadorIA.Models;

public class Cotizacion
{
    public int Id { get; set; }
    public string Folio { get; set; } = string.Empty;
    public int ClienteId { get; set; }
    public int? UsuarioId { get; set; }
    public DateTime FechaCotizacion { get; set; } = DateTime.Today;
    public string NombreCliente { get; set; } = string.Empty;
    public string? CodigoSapCliente { get; set; }
    public string? ReferenciaCliente { get; set; }
    public string? DireccionCliente { get; set; }
    public string NombreVendedor { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaVigencia { get; set; }
    public string Moneda { get; set; } = "USD";
    public decimal? TipoCambio { get; set; }
    public string? Cultivo { get; set; }
    public string? CropGroup { get; set; }
    public decimal PorcentajeDescuento { get; set; }
    public decimal PorcentajeIva { get; set; }
    public decimal Subtotal { get; set; }
    public decimal MontoDescuento { get; set; }
    public decimal MontoIva { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = "Borrador";

    public Cliente Cliente { get; set; } = null!;
    public Usuario? Usuario { get; set; }
    public ICollection<CotizacionDetalle> Detalles { get; set; } = new List<CotizacionDetalle>();
}
