using ECommerce.Blazor_WebApp.Client.Services.API.Implementations;
using ECommerce.Blazor_WebApp.Client.Services.API.Interfaces;
using ECommerce.Blazor_WebApp.Client.Services.State;
using ECommerce.Blazor_WebApp.Components;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<SharedStateService>();

builder.Services.AddRadzenComponents();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationCore();

builder.Services.AddHttpClient<IShoppingCartApi, ShoppingCartApi>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Api:BaseUrl"]!);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ECommerce.Blazor_WebApp.Client._Imports).Assembly);

app.Run();
