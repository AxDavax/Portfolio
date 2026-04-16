namespace ECommerce.ClientPortal.Services.API;

public static class ApiServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, 
                                                         IConfiguration config)
    {
        var apiUrl = config["Api:BaseUrl"];

        services.AddHttpClient("API", client =>
        {
            client.BaseAddress = new Uri(apiUrl!);
        });

        services.AddScoped(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            return factory.CreateClient("API");
        });

        ApiConfiguration.AddApiServices(services);

        return services;
    }
}