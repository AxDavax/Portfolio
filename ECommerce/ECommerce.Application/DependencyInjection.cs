using ECommerce.Application.Catalog.Interfaces;
using ECommerce.Application.Catalog.Services;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Interfaces.Auth;
using ECommerce.Application.Services;
using ECommerce.Application.Services.Auth;
using ECommerce.Application.UseCases.Auth.ForgotPassword;
using ECommerce.Application.UseCases.Auth.Login;
using ECommerce.Application.UseCases.Auth.Logout;
using ECommerce.Application.UseCases.Auth.Me;
using ECommerce.Application.UseCases.Auth.Refresh;
using ECommerce.Application.UseCases.Auth.Register;
using ECommerce.Application.UseCases.Auth.ResetPassword;
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
        services.AddScoped<RegisterHandler>();
        services.AddScoped<LogoutHandler>();
        services.AddScoped<ForgotPasswordHandler>();
        services.AddScoped<ResetPasswordHandler>();

        // Services
        services.AddScoped<ICartToOrder, CartToOrder>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IShoppingCartService, ShoppingCartService>();
        services.AddScoped<IOrderService, OrderService>();

        // Auth Services
        services.AddScoped<IExternalLoginService, ExternalLoginService>();

        return services;
    }
}