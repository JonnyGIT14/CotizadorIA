namespace CotizadorIA.Models;

public class SubfamiliaProducto
{
    public int Id { get; set; }
    public int FamiliaProductoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activa { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;

    public FamiliaProducto Familia { get; set; } = null!;
    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
