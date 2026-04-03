using ECommerce.Blazor_WebApp.Client.Services.API.Interfaces;

namespace ECommerce.Blazor_WebApp.Client.Services.API;

public static class ApiConfiguration
{
    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryApi, ICategoryApi>();
    }
}