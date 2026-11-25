using CotizadorIA.Models;
using Microsoft.EntityFrameworkCore;

namespace CotizadorIA.Data;

public class ClienteData
{
    private readonly ApplicationDbContext _context;

    public ClienteData(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Cliente>> ObtenerClientesAsync(
        bool incluirInactivos = false,
        CancellationToken cancellationToken = default)
    {
        var consulta = _context.Set<Cliente>().AsQueryable();
        if (!incluirInactivos)
        {
            consulta = consulta.Where(x => x.Activo);
        }

        return await consulta
            .OrderBy(x => x.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<Cliente?> ObtenerClienteAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Cliente>().FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Cliente?> ObtenerPorCodigoSapAsync(
        string codigoSap,
        CancellationToken cancellationToken = default)
    {
        var codigo = codigoSap.Trim();
        return await _context.Set<Cliente>()
            .FirstOrDefaultAsync(x => x.CodigoSap == codigo, cancellationToken);
    }

    public async Task<Cliente> CrearClienteAsync(Cliente datos, CancellationToken cancellationToken = default)
    {
        var codigoSap = LimpiarOpcional(datos.CodigoSap);
        if (codigoSap is not null
            && await _context.Set<Cliente>().AnyAsync(x => x.CodigoSap == codigoSap, cancellationToken))
        {
            throw new InvalidOperationException($"Ya existe un cliente con el código SAP '{codigoSap}'.");
        }

        var ahora = DateTime.UtcNow;
        datos.ClaveImportacion = string.IsNullOrWhiteSpace(datos.ClaveImportacion)
            ? Guid.NewGuid().ToString("N")
            : datos.ClaveImportacion.Trim();
        datos.CodigoSap = codigoSap;
        datos.Nombre = datos.Nombre.Trim();
        datos.Direccion = LimpiarOpcional(datos.Direccion);
        datos.FechaCreacion = ahora;
        datos.FechaModificacion = ahora;
        datos.Activo = true;
        _context.Set<Cliente>().Add(datos);
        await _context.SaveChangesAsync(cancellationToken);
        return datos;
    }

    public async Task<Cliente?> ActualizarClienteAsync(
        int id,
        Cliente datos,
        CancellationToken cancellationToken = default)
    {
        var existente = await _context.Set<Cliente>().FindAsync(new object[] { id }, cancellationToken);
        if (existente is null)
        {
            return null;
        }

        var codigoSap = LimpiarOpcional(datos.CodigoSap);
        if (codigoSap is not null
            && await _context.Set<Cliente>().AnyAsync(
                x => x.Id != id && x.CodigoSap == codigoSap,
                cancellationToken))
        {
            throw new InvalidOperationException($"Ya existe un cliente con el código SAP '{codigoSap}'.");
        }

        existente.CodigoSap = codigoSap;
        existente.Categoria = datos.Categoria;
        existente.Nombre = datos.Nombre.Trim();
        existente.FactorGeneral = datos.FactorGeneral;
        existente.FactorStreamline = datos.FactorStreamline;
        existente.Direccion = LimpiarOpcional(datos.Direccion);
        existente.Activo = datos.Activo;
        existente.FechaModificacion = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return existente;
    }

    private static string? LimpiarOpcional(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }
}
