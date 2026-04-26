using ECommerce.ClientPortal.ViewModels.Auth;
using ECommerce.ClientPortal.ViewModels.Categories;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.ClientPortal.ViewModels.Home;
using ECommerce.ClientPortal.ViewModels.Products;

namespace ECommerce.ClientPortal.Extensions;

public static class VMsConfiguration
{
    public static void AddViewModels(this IServiceCollection services)
    {
        // Core VMs
        services.AddScoped<AuthUserVM>();
        services.AddScoped<HomeVM>();

        // Auth VMs
        services.AddScoped<LoginVM>();
        services.AddScoped<RegisterVM>();
        services.AddScoped<LogoutVM>();
        services.AddScoped<UserMenuVM>();
        services.AddScoped<ProfileVM>();
        services.AddScoped<ForgotPasswordVM>();
        services.AddScoped<ResetPasswordVM>();

        // Category VMs
        services.AddScoped<CategoryListVM>();
        services.AddScoped<CategoryUpsertVM>();

        // Product VMs
        services.AddScoped<ProductListVM>();
        services.AddScoped<ProductUpsertVM>();
    }
}