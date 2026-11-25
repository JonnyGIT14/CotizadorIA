using CotizadorIA.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CotizadorIA.Services;

public static class CatalogoServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogoBackend(this IServiceCollection services)
    {
        services.AddScoped<ProductoData>();
        services.AddScoped<ClienteData>();
        services.AddScoped<CatalogoImportService>();
        services.AddScoped<CotizacionCalculator>();
        return services;
    }
}
