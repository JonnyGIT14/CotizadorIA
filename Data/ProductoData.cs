using CotizadorIA.Models;
using Microsoft.EntityFrameworkCore;

namespace CotizadorIA.Data;

public class ProductoData
{
    private readonly ApplicationDbContext _context;

    public ProductoData(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Producto>> ObtenerProductosAsync(
        bool incluirInactivos = false,
        CancellationToken cancellationToken = default)
    {
        var consulta = _context.Set<Producto>()
            .Include(x => x.Subfamilia)
            .ThenInclude(x => x.Familia)
            .AsQueryable();

        if (!incluirInactivos)
        {
            consulta = consulta.Where(x => x.Activo && x.Subfamilia.Activa && x.Subfamilia.Familia.Activa);
        }

        return await consulta
            .OrderBy(x => x.Subfamilia.Familia.Nombre)
            .ThenBy(x => x.Subfamilia.Nombre)
            .ThenBy(x => x.CodigoCatalogo)
            .ToListAsync(cancellationToken);
    }

    public async Task<Producto?> ObtenerProductoAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Producto>()
            .Include(x => x.Subfamilia)
            .ThenInclude(x => x.Familia)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Producto?> ObtenerPorCodigoAsync(
        string codigoCatalogo,
        CancellationToken cancellationToken = default)
    {
        var codigo = codigoCatalogo.Trim();
        return await _context.Set<Producto>()
            .Include(x => x.Subfamilia)
            .ThenInclude(x => x.Familia)
            .FirstOrDefaultAsync(x => x.CodigoCatalogo == codigo, cancellationToken);
    }

    public async Task<Producto> CrearProductoAsync(Producto datos, CancellationToken cancellationToken = default)
    {
        var codigo = datos.CodigoCatalogo.Trim();
        if (await _context.Set<Producto>().AnyAsync(x => x.CodigoCatalogo == codigo, cancellationToken))
        {
            throw new InvalidOperationException($"Ya existe el SKU '{codigo}'.");
        }

        var ahora = DateTime.UtcNow;
        datos.CodigoCatalogo = codigo;
        datos.Codigo = codigo;
        datos.Nombre = datos.Descripcion.Trim();
        datos.Precio = datos.PrecioLista;
        datos.FechaCreacion = ahora;
        datos.FechaModificacion = ahora;
        datos.Activo = true;
        _context.Set<Producto>().Add(datos);
        await _context.SaveChangesAsync(cancellationToken);
        return datos;
    }

    public async Task<Producto?> ActualizarProductoAsync(
        int id,
        Producto datos,
        CancellationToken cancellationToken = default)
    {
        var existente = await _context.Set<Producto>().FindAsync(new object[] { id }, cancellationToken);
        if (existente is null)
        {
            return null;
        }

        var codigo = datos.CodigoCatalogo.Trim();
        if (await _context.Set<Producto>().AnyAsync(
                x => x.Id != id && x.CodigoCatalogo == codigo,
                cancellationToken))
        {
            throw new InvalidOperationException($"Ya existe el SKU '{codigo}'.");
        }

        existente.CodigoCatalogo = codigo;
        existente.Descripcion = datos.Descripcion.Trim();
        existente.Codigo = codigo;
        existente.Nombre = existente.Descripcion;
        existente.Unidad = datos.Unidad.Trim();
        existente.SubfamiliaProductoId = datos.SubfamiliaProductoId;
        existente.Calibre = datos.Calibre?.Trim();
        existente.Caudal = datos.Caudal?.Trim();
        existente.Espaciamiento = datos.Espaciamiento?.Trim();
        existente.Clasificacion = datos.Clasificacion.Trim();
        existente.SemanasEntrega = datos.SemanasEntrega;
        existente.CantidadMinima = datos.CantidadMinima;
        existente.CantidadMinimaTexto = datos.CantidadMinimaTexto?.Trim();
        existente.PrecioLista = datos.PrecioLista;
        existente.Precio = datos.PrecioLista;
        existente.Activo = datos.Activo;
        existente.FechaModificacion = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return existente;
    }
}
