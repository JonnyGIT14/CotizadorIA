using System.ComponentModel.DataAnnotations;

namespace CotizadorIA.Models;

public class CreateUsuarioViewModel
{
    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string ApellidoPaterno { get; set; } = string.Empty;

    public string? ApellidoMaterno { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un rol.")]
    public int RolId { get; set; }
}
