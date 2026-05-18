using ECommerce.Application.OAuth.Interfaces;
using ECommerce.Application.OAuth.Services;
using ECommerce.Domain.OAuth.Interfaces;
using ECommerce.Infrastructure.OAuth.Configuration;
using ECommerce.Infrastructure.OAuth.Providers;
using ECommerce.Infrastructure.OAuth.Repositories;
using ECommerce.Infrastructure.OAuth.StateStores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.OAuth;

public static class OAuthDependencyInjection
{
    public static IServiceCollection AddOAuthInfrastructure(this IServiceCollection services,
            IConfiguration config)
    {
        // HttpClient factory
        services.AddHttpClient();

        // Provider configuration
        services.AddOAuthProviders(config);

        // Repositories
        services.AddScoped<IUserLoginRepository, UserLoginRepository>();

        // Providers
        services.AddScoped<IExternalAuthProvider, GoogleAuthProvider>();
        services.AddScoped<IExternalAuthProvider, FacebookAuthProvider>();
        services.AddScoped<IExternalAuthProvider, MicrosoftAuthProvider>();

        // State store
        services.AddSingleton<IExternalLoginStateStore, ExternalLoginStateStore>();

        // Principal service in Application layer
        services.AddScoped<IExternalLoginService, ExternalLoginService>();

        return services;
    }
}