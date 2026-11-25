namespace CotizadorIA.Models;

public class Cliente
{
    public int Id { get; set; }
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string ClaveImportacion { get; set; } = string.Empty;
    public string? CodigoSap { get; set; }
    public CategoriaCliente? Categoria { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal? FactorGeneral { get; set; }
    public decimal? FactorStreamline { get; set; }
    public string? Direccion { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
}
