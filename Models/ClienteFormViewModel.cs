using System.ComponentModel.DataAnnotations;

namespace CotizadorIA.Models;

public class ClienteFormViewModel
{
    public int Id { get; set; }

    [Required, StringLength(250)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public CategoriaCliente? Categoria { get; set; }

    [StringLength(50)]
    public string? CodigoSap { get; set; }

    [Range(typeof(decimal), "0", "1")]
    public decimal? FactorGeneral { get; set; }

    [Range(typeof(decimal), "0", "1")]
    public decimal? FactorStreamline { get; set; }

    [StringLength(1000)]
    public string? Direccion { get; set; }

    [EmailAddress, StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [StringLength(100)]
    public string Telefono { get; set; } = string.Empty;

    [StringLength(150)]
    public string Apellido { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;
}
