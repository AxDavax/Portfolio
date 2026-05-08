using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.Services.State;
using ECommerce.ClientPortal.Services.Storage;

namespace ECommerce.ClientPortal.Extensions;

public static class AppServicesExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        // State Management
        services.AddSingleton<CartState>();

        // App Services
        services.AddScoped<UserSessionService>();
        services.AddScoped<LocalStorageService>();
        services.AddScoped<TokenStorageService>();
        services.AddScoped<AuthService>();

        return services;
    }
}