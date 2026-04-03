using ECommerce.Blazor_WebApp.Client.Services.API.Implementations;
using ECommerce.Blazor_WebApp.Client.Services.API.Interfaces;

namespace ECommerce.Blazor_WebApp.Client.Services.API;

public static class ApiConfiguration
{
    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryApi, CategoryApi>();
        services.AddScoped<IProductApi, ProductApi>();
        services.AddScoped<IShoppingCartApi, ShoppingCartApi>();
    }
}