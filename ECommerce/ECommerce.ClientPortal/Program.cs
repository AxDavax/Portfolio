using ECommerce.ClientPortal;
using ECommerce.ClientPortal.Extensions;
using ECommerce.ClientPortal.Providers;
using ECommerce.ClientPortal.Services.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.HostEnvironment.Environment}.json", optional: true, reloadOnChange: true);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();

builder.Services.AddTransient<AuthHttpMessageHandler>();

builder.Services.AddHttpClient("AuthorizedClient")
    .AddHttpMessageHandler<AuthHttpMessageHandler>();

builder.Services.AddScoped(sp =>
{
    var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return clientFactory.CreateClient("AuthorizedClient");
});

builder.Services.AddAppServices();

builder.Services.AddApiServices(builder.Configuration);

builder.Services.AddViewModels();

builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddRadzenComponents();

await builder.Build().RunAsync();