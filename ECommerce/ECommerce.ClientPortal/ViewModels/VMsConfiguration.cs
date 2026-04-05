using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.ClientPortal.ViewModels.Home;

namespace ECommerce.ClientPortal.ViewModels;

public static class VMsConfiguration
{
    public static void AddViewModels(this IServiceCollection services)
    {
        services.AddScoped<AuthUserVM>();
        services.AddScoped<HomeVM>();
    }
}