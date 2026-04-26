using ECommerce.ClientPortal.Services.API.Implementations;
using ECommerce.ClientPortal.Services.API.Interfaces;

namespace ECommerce.ClientPortal.Extensions;

public static class ApiConfiguration
{
    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryApi, CategoryApi>();
        services.AddScoped<IProductApi, ProductApi>();
        services.AddScoped<IShoppingCartApi, ShoppingCartApi>();
        services.AddScoped<IOrderApi, OrderApi>();
        services.AddScoped<ICartApi, CartApi>();
        services.AddScoped<IPaymentApi, PaymentApi>();
        services.AddScoped<IFileApi, FileApi>();
    }
}