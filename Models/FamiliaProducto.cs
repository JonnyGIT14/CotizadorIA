namespace CotizadorIA.Models;

public class FamiliaProducto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activa { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;

    public ICollection<SubfamiliaProducto> Subfamilias { get; set; } = new List<SubfamiliaProducto>();
}
