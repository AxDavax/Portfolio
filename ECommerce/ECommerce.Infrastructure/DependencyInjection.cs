using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Auth;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Data
        services.AddSingleton<DapperContext>();
        services.AddSingleton<ISqlDataAccess, SqlDataAccess>();

        // Repositories
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Auth 
        services.AddSingleton<IJwtService, JwtService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();

        // Services
        services.AddScoped<IFileService, FileService>();
        services.AddSingleton<IPasswordService, PasswordService>();
        services.AddScoped<IPaymentService, PaymentService>();
        
        return services;
    }
}