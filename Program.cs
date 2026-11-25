using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using CotizadorIA.Data;
using CotizadorIA.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.SameAsRequest
            : CookieSecurePolicy.Always;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<RolData>();
builder.Services.AddScoped<UsuarioData>();
builder.Services.AddCatalogoBackend();
builder.Services.AddHttpClient<FrankfurterExchangeRateService>(client =>
{
    client.BaseAddress = new Uri("https://api.frankfurter.dev/");
    client.Timeout = TimeSpan.FromSeconds(5);
});

var app = builder.Build();

if (args.Length > 0 && args[0] == "--import-catalog")
{
    if (args.Length != 3)
    {
        throw new ArgumentException(
            "Uso: dotnet run -- --import-catalog <ruta-prices.json> <ruta-clients.json>");
    }

    await using var importScope = app.Services.CreateAsyncScope();
    var dbContext = importScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();

    var importador = importScope.ServiceProvider.GetRequiredService<CatalogoImportService>();
    var resultado = await importador.ImportarAsync(args[1], args[2]);
    Console.WriteLine(
        $"Importación completada: {resultado.ProductosNuevos} productos nuevos, "
        + $"{resultado.ProductosActualizados} actualizados; "
        + $"{resultado.ClientesNuevos} clientes nuevos, "
        + $"{resultado.ClientesActualizados} actualizados; "
        + $"{resultado.FamiliasNuevas} familias y {resultado.SubfamiliasNuevas} subfamilias nuevas.");
    return;
}

using (var scope = app.Services.CreateScope())
{
    var rolData = scope.ServiceProvider.GetRequiredService<RolData>();
    var usuarioData = scope.ServiceProvider.GetRequiredService<UsuarioData>();

    await rolData.InicializarAsync();

}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
