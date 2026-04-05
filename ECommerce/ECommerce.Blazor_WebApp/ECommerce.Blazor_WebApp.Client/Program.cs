using ECommerce.Blazor_WebApp.Client.Services.API;
using ECommerce.Blazor_WebApp.Client.Services.State;
using ECommerce.Blazor_WebApp.Client.ViewModels;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient();
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddScoped<SharedStateService>();
builder.Services.AddViewModels();
builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();
