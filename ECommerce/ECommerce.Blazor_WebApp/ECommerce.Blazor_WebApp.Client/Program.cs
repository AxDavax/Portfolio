using ECommerce.Blazor_WebApp.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<SharedStateService>();
builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();
