namespace ECommerce.Blazor_WebApp.Client.Services.API;

public static class ApiServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
    {
        var apiUrl = config["Api:BaseUrl"];

        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<HttpClient>();
            client.BaseAddress = new Uri(apiUrl!);
            return client;
        });

        ApiConfiguration.AddApiServices(services);

        return services;
    }
}