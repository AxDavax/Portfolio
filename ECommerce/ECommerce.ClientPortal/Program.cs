using ECommerce.ClientPortal;
using ECommerce.ClientPortal.Providers;
using ECommerce.ClientPortal.Services.API;
using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.Services.State;
using ECommerce.ClientPortal.Services.Storage;
using ECommerce.ClientPortal.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddHttpClient();
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddScoped<SharedStateService>();
builder.Services.AddScoped<LocalStorageService>();

builder.Services.AddScoped<TokenStorageService>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<AuthService>();

builder.Services.AddViewModels();
builder.Services.AddAuthorizationCore();

var host = builder.Build();

var authService = host.Services.GetRequiredService<AuthService>();
await authService.InitializeAsync();

await host.RunAsync();