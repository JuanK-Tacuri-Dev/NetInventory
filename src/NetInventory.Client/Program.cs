using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NetInventory.Client;
using NetInventory.Client.Services;

// Forzar cultura invariante para que los números usen siempre punto decimal
// independientemente de la configuración regional del navegador del usuario.
var invariant = System.Globalization.CultureInfo.InvariantCulture;
System.Globalization.CultureInfo.DefaultThreadCurrentCulture   = invariant;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = invariant;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5001";
    return new HttpClient { BaseAddress = new Uri(apiBaseUrl) };
});

// Lazy<ApiClientService> para romper el ciclo: TokenStoreService → ApiClientService
builder.Services.AddScoped(sp => new Lazy<ApiClientService>(sp.GetRequiredService<ApiClientService>));

builder.Services.AddScoped<TokenStoreService>();
builder.Services.AddScoped<ApiClientService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<AuthService>());
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<GeneralValueService>();
builder.Services.AddScoped<ErrorLogService>();
builder.Services.AddScoped<AuditLogService>();
builder.Services.AddScoped<AuditConfigService>();
builder.Services.AddScoped<InventoryEventService>();

await builder.Build().RunAsync();
