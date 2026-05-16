using ECommerce.Application.OAuth.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.OAuth.Configuration;

public static class OAuthProvidersConfiguration
{
    public static IServiceCollection AddOAuthProviders(
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