using ECommerce.Infrastructure.Auth;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repositories.Catalog;
using ECommerce.Infrastructure.Repositories.Orders;
using ECommerce.Infrastructure.Repositories.Cart;
using ECommerce.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.Domain.Catalog.Interfaces;
using ECommerce.Domain.Cart.Interfaces;
using ECommerce.Domain.Orders.Interfaces;
using ECommerce.Domain.Auth.Interfaces;
using ECommerce.Domain.Data.Interfaces;
using ECommerce.Application.Auth.Interfaces;
using ECommerce.Application.OAuth.Interfaces;
using ECommerce.Application.Files.Interfaces;
using ECommerce.Application.Payments.Interfaces;
using ECommerce.Infrastructure.Auth.Repositories;

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

        // Auth Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserAuthRepository, UserRepository>();
        services.AddScoped<IUserLoginRepository, UserLoginRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IResetPasswordTokenRepository, ResetPasswordTokenRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();

        // Auth Services
        services.AddSingleton<IJwtService, JwtService>();
        services.AddSingleton<IPasswordService, PasswordService>();

        services.AddSingleton<IExternalLoginStateStore, ExternalLoginStateStore>();

        // Services
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IPaymentService, PaymentService>();

        return services;
    }
}