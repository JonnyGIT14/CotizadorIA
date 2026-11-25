using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CotizadorIA.Models;
using Microsoft.EntityFrameworkCore;

namespace CotizadorIA.Data;

public class CatalogoImportService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = false
    };

    private readonly ApplicationDbContext _context;

    public CatalogoImportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ResultadoImportacionCatalogo> ImportarAsync(
        string rutaProductos,
        string rutaClientes,
        CancellationToken cancellationToken = default)
    {
        var productosImportados = await LeerJsonAsync<ProductoImportDto>(rutaProductos, cancellationToken);
        var clientesImportados = await LeerJsonAsync<ClienteImportDto>(rutaClientes, cancellationToken);
        Validar(productosImportados, clientesImportados);

        await using var transaccion = await _context.Database.BeginTransactionAsync(cancellationToken);
        var resultado = new ResultadoImportacionCatalogo();
        var ahora = DateTime.UtcNow;

        var familias = await _context.Set<FamiliaProducto>().ToListAsync(cancellationToken);
        var mapaFamilias = familias.ToDictionary(x => NormalizarClave(x.Nombre), x => x);

        foreach (var producto in productosImportados)
        {
            var clave = NormalizarClave(producto.Familia!);
            if (!mapaFamilias.ContainsKey(clave))
            {
                var familia = new FamiliaProducto
                {
                    Nombre = producto.Familia!.Trim(),
                    FechaCreacion = ahora,
                    FechaModificacion = ahora
                };
                _context.Set<FamiliaProducto>().Add(familia);
                mapaFamilias.Add(clave, familia);
                resultado.FamiliasNuevas++;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        var subfamilias = await _context.Set<SubfamiliaProducto>().ToListAsync(cancellationToken);
        var mapaSubfamilias = subfamilias.ToDictionary(
            x => ClaveSubfamilia(x.FamiliaProductoId, x.Nombre),
            x => x);

        foreach (var producto in productosImportados)
        {
            var familia = mapaFamilias[NormalizarClave(producto.Familia!)];
            var clave = ClaveSubfamilia(familia.Id, producto.Subfamilia!);
            if (!mapaSubfamilias.ContainsKey(clave))
            {
                var subfamilia = new SubfamiliaProducto
                {
                    FamiliaProductoId = familia.Id,
                    Nombre = producto.Subfamilia!.Trim(),
                    FechaCreacion = ahora,
                    FechaModificacion = ahora
                };
                _context.Set<SubfamiliaProducto>().Add(subfamilia);
                mapaSubfamilias.Add(clave, subfamilia);
                resultado.SubfamiliasNuevas++;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        var productos = await _context.Set<Producto>().ToDictionaryAsync(
            x => x.CodigoCatalogo,
            StringComparer.OrdinalIgnoreCase,
            cancellationToken);

        foreach (var importado in productosImportados)
        {
            var codigo = importado.CodigoCatalogo!.Trim();
            var esNuevo = !productos.TryGetValue(codigo, out var producto);
            if (esNuevo)
            {
                producto = new Producto
                {
                    CodigoCatalogo = codigo,
                    FechaCreacion = ahora
                };
                productos.Add(codigo, producto);
                _context.Set<Producto>().Add(producto);
                resultado.ProductosNuevos++;
            }
            else
            {
                resultado.ProductosActualizados++;
            }

            producto!.Descripcion = importado.Descripcion!.Trim();
            producto.Codigo = producto.CodigoCatalogo;
            producto.Nombre = producto.Descripcion;
            producto.Unidad = importado.Unidad!.Trim();
            producto.SubfamiliaProductoId = mapaSubfamilias[
                ClaveSubfamilia(mapaFamilias[NormalizarClave(importado.Familia!)].Id, importado.Subfamilia!)].Id;
            producto.Calibre = LimpiarOpcional(importado.Calibre);
            producto.Caudal = LimpiarOpcional(importado.Caudal);
            producto.Espaciamiento = LimpiarOpcional(importado.Espaciamiento);
            producto.Clasificacion = importado.Clasificacion!.Trim();
            producto.SemanasEntrega = importado.SemanasEntrega;
            producto.CantidadMinima = importado.CantidadMinima;
            producto.CantidadMinimaTexto = LimpiarOpcional(importado.CantidadMinimaTexto);
            producto.PrecioLista = importado.PrecioLista!.Value;
            producto.Precio = producto.PrecioLista;
            producto.FechaModificacion = ahora;
        }

        var clientes = await _context.Set<Cliente>().ToDictionaryAsync(
            x => x.ClaveImportacion,
            StringComparer.Ordinal,
            cancellationToken);

        foreach (var importado in clientesImportados)
        {
            var claveImportacion = CrearClaveImportacion(importado);
            var esNuevo = !clientes.TryGetValue(claveImportacion, out var cliente);
            if (esNuevo)
            {
                cliente = new Cliente
                {
                    ClaveImportacion = claveImportacion,
                    FechaCreacion = ahora
                };
                clientes.Add(claveImportacion, cliente);
                _context.Set<Cliente>().Add(cliente);
                resultado.ClientesNuevos++;
            }
            else
            {
                resultado.ClientesActualizados++;
            }

            cliente!.CodigoSap = LimpiarOpcional(importado.CodigoSap);
            cliente.Categoria = ConvertirCategoria(importado.Categoria!);
            cliente.Nombre = importado.Nombre!.Trim();
            cliente.FactorGeneral = importado.FactorGeneral!.Value;
            cliente.FactorStreamline = importado.FactorStreamline;
            cliente.Direccion = LimpiarOpcional(importado.Direccion);
            cliente.FechaModificacion = ahora;
        }

        await _context.SaveChangesAsync(cancellationToken);
        await transaccion.CommitAsync(cancellationToken);
        return resultado;
    }

    private static async Task<List<T>> LeerJsonAsync<T>(string ruta, CancellationToken cancellationToken)
    {
        if (!File.Exists(ruta))
        {
            throw new FileNotFoundException("No se encontró el archivo JSON del catálogo.", ruta);
        }

        var json = await File.ReadAllTextAsync(ruta, cancellationToken);
        return JsonSerializer.Deserialize<List<T>>(json, JsonOptions)
            ?? throw new InvalidDataException($"El archivo '{ruta}' no contiene una lista JSON válida.");
    }

    private static void Validar(
        IReadOnlyCollection<ProductoImportDto> productos,
        IReadOnlyCollection<ClienteImportDto> clientes)
    {
        var errores = new List<string>();
        if (productos.Count == 0)
        {
            errores.Add("El archivo de productos está vacío.");
        }

        if (clientes.Count == 0)
        {
            errores.Add("El archivo de clientes está vacío.");
        }

        for (var i = 0; i < productos.Count; i++)
        {
            var p = productos.ElementAt(i);
            var fila = i + 1;
            if (string.IsNullOrWhiteSpace(p.CodigoCatalogo)
                || string.IsNullOrWhiteSpace(p.Descripcion)
                || string.IsNullOrWhiteSpace(p.Unidad)
                || string.IsNullOrWhiteSpace(p.Familia)
                || string.IsNullOrWhiteSpace(p.Subfamilia)
                || string.IsNullOrWhiteSpace(p.Clasificacion)
                || p.PrecioLista is null or < 0
                || p.SemanasEntrega is < 0
                || p.CantidadMinima is < 0)
            {
                errores.Add($"Producto, fila {fila}: faltan campos requeridos o hay un valor negativo.");
            }
        }

        AgregarDuplicados(
            productos.Select(x => x.CodigoCatalogo?.Trim() ?? string.Empty),
            "código de producto",
            errores);

        for (var i = 0; i < clientes.Count; i++)
        {
            var c = clientes.ElementAt(i);
            var fila = i + 1;
            if (string.IsNullOrWhiteSpace(c.Categoria)
                || string.IsNullOrWhiteSpace(c.Nombre)
                || c.FactorGeneral is null or < 0 or > 1
                || c.FactorStreamline is < 0 or > 1)
            {
                errores.Add($"Cliente, fila {fila}: faltan campos requeridos o los factores están fuera del rango 0 a 1.");
                continue;
            }

            try
            {
                _ = ConvertirCategoria(c.Categoria);
            }
            catch (InvalidDataException ex)
            {
                errores.Add($"Cliente, fila {fila}: {ex.Message}");
            }
        }

        AgregarDuplicados(
            clientes.Select(CrearClaveImportacion),
            "clave de importación de cliente",
            errores);

        if (errores.Count > 0)
        {
            throw new InvalidDataException(
                "La importación se canceló. Corrige estos datos antes de volver a intentarlo:"
                + Environment.NewLine
                + string.Join(Environment.NewLine, errores));
        }
    }

    private static void AgregarDuplicados(IEnumerable<string> claves, string campo, ICollection<string> errores)
    {
        var duplicados = claves
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key);

        foreach (var duplicado in duplicados)
        {
            errores.Add($"Valor duplicado en {campo}: '{duplicado}'.");
        }
    }

    private static string CrearClaveImportacion(ClienteImportDto cliente)
    {
        if (!string.IsNullOrWhiteSpace(cliente.CodigoSap))
        {
            return "SAP:" + NormalizarClave(cliente.CodigoSap);
        }

        var identidad = string.Join("|",
            NormalizarClave(cliente.Categoria!),
            NormalizarClave(cliente.Nombre!),
            NormalizarClave(cliente.Direccion ?? string.Empty));
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(identidad)));
        return "SIN-SAP:" + hash;
    }

    private static CategoriaCliente ConvertirCategoria(string categoria)
    {
        return categoria.Trim().ToUpperInvariant() switch
        {
            "DEALER" => CategoriaCliente.Dealer,
            "SUB DEALER" => CategoriaCliente.SubDealer,
            "CLIENTE FINAL" => CategoriaCliente.ClienteFinal,
            _ => throw new InvalidDataException($"Categoría de cliente no reconocida: '{categoria}'.")
        };
    }

    private static string NormalizarClave(string valor)
    {
        var normalizado = valor.Trim().Normalize(NormalizationForm.FormD);
        var sinAcentos = new string(normalizado
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .ToArray());
        return string.Join(" ", sinAcentos.ToUpperInvariant()
            .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
    }

    private static string ClaveSubfamilia(int familiaId, string nombre)
    {
        return familiaId + "|" + NormalizarClave(nombre);
    }

    private static string? LimpiarOpcional(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }

    private sealed class ProductoImportDto
    {
        [JsonPropertyName("fam")] public string? Familia { get; set; }
        [JsonPropertyName("sub")] public string? Subfamilia { get; set; }
        [JsonPropertyName("cat")] public string? CodigoCatalogo { get; set; }
        [JsonPropertyName("desc")] public string? Descripcion { get; set; }
        [JsonPropertyName("un")] public string? Unidad { get; set; }
        [JsonPropertyName("calibre")] public string? Calibre { get; set; }
        [JsonPropertyName("caudal")] public string? Caudal { get; set; }
        [JsonPropertyName("esp")] public string? Espaciamiento { get; set; }
        [JsonPropertyName("clas")] public string? Clasificacion { get; set; }
        [JsonPropertyName("entrega")] public decimal? SemanasEntrega { get; set; }
        [JsonPropertyName("moq")] public decimal? CantidadMinima { get; set; }
        [JsonPropertyName("moq_txt")] public string? CantidadMinimaTexto { get; set; }
        [JsonPropertyName("precio")] public decimal? PrecioLista { get; set; }
    }

    private sealed class ClienteImportDto
    {
        [JsonPropertyName("cat")] public string? Categoria { get; set; }
        [JsonPropertyName("nombre")] public string? Nombre { get; set; }
        [JsonPropertyName("fg")] public decimal? FactorGeneral { get; set; }
        [JsonPropertyName("fs")] public decimal? FactorStreamline { get; set; }
        [JsonPropertyName("dir")] public string? Direccion { get; set; }
        [JsonPropertyName("sap")] public string? CodigoSap { get; set; }
    }
}
