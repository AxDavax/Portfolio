using ECommerce.ClientPortal;
using ECommerce.ClientPortal.Providers;
using ECommerce.ClientPortal.Services.API;
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
builder.Services.AddViewModels();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

await builder.Build().RunAsync();