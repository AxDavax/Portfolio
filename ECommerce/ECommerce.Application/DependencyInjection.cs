using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using ECommerce.Application.UseCases.Auth.Login;
using ECommerce.Application.UseCases.Auth.Me;
using ECommerce.Application.UseCases.Auth.Refresh;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Auth Use Cases
        services.AddScoped<LoginHandler>();
        services.AddScoped<RefreshHandler>();
        services.AddScoped<MeHandler>();

        // Services
        services.AddScoped<ICartToOrder, CartToOrder>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IShoppingCartService, ShoppingCartService>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}