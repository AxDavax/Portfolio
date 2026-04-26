using ECommerce.ClientPortal.Services.Http;

namespace ECommerce.ClientPortal.Extensions;

public static class ApiServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, 
                                                         IConfiguration config)
    {
        services.AddTransient<AuthHttpMessageHandler>();
        
        var apiUrl = config["Api:BaseUrl"];

        services.AddHttpClient("AuthorizedClient", client => {
            client.BaseAddress = new Uri(apiUrl!);
        })
        .AddHttpMessageHandler<AuthHttpMessageHandler>();

        services.AddScoped(sp =>
        {
            var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
            return clientFactory.CreateClient("AuthorizedClient");
        });

        ApiConfiguration.AddApiServices(services);

        return services;
    }
}