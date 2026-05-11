using ECommerce.Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Auth;

public static class AuthProvidersConfiguration
{
    public static IServiceCollection AddAuthProviders(
            this IServiceCollection services,
            IConfiguration config)
    {
        services.Configure<ProviderSettings>("Google",
            config.GetSection("Authentication:Google"));

        services.Configure<ProviderSettings>("Facebook",
            config.GetSection("Authentication:Facebook"));

        services.Configure<ProviderSettings>("Microsoft",
            config.GetSection("Authentication:Microsoft"));

        return services;
    }
}