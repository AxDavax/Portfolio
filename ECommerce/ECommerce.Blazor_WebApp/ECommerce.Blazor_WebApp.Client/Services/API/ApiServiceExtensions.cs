using ECommerce.Blazor_WebApp.Client.Services.API.Implementations;
using ECommerce.Blazor_WebApp.Client.Services.API.Interfaces;

namespace ECommerce.Blazor_WebApp.Client.Services.API;

public static class ApiServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
    {
        var apiUrl = config["Api:BaseUrl"];

        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<HttpClient>();
            client.BaseAddress = new Uri(apiUrl!);
            return client;
        });

        services.AddScoped<ICategoryApi, CategoryApi>();
        services.AddScoped<IProductApi, ProductApi>();
        services.AddScoped<IShoppingCartApi, ShoppingCartApi>();
        services.AddScoped<IOrderApi, OrderApi>();
        services.AddScoped<ICartApi, CartApi>();
        services.AddScoped<IPaymentApi, PaymentApi>();
        services.AddScoped<IFileApi, FileApi>();

        return services;
    }
}