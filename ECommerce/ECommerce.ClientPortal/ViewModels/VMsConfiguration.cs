using ECommerce.ClientPortal.ViewModels.Auth;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.ClientPortal.ViewModels.Home;

namespace ECommerce.ClientPortal.ViewModels;

public static class VMsConfiguration
{
    public static void AddViewModels(this IServiceCollection services)
    {
        services.AddScoped<AuthUserVM>();
        services.AddScoped<HomeVM>();

        services.AddScoped<LoginVM>();
        services.AddScoped<RegisterVM>();
        services.AddScoped<LogoutVM>();
        services.AddScoped<UserMenuVM>();
        services.AddScoped<ProfileVM>();
        services.AddScoped<ForgotPasswordVM>();
        services.AddScoped<ResetPasswordVM>();
    }
}