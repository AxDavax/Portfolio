using ECommerce.Blazor_WebApp.Client.ViewModels.Core;
using ECommerce.Blazor_WebApp.Client.ViewModels.Home;

namespace ECommerce.Blazor_WebApp.Client.ViewModels;

public static class VMsConfiguration
{
    public static void AddViewModels(this IServiceCollection services)
    {
        services.AddScoped<AuthUserVM>();
        services.AddScoped<HomeVM>();
    }
}