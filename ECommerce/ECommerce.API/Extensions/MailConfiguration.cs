using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure.Services;
using Polly;
using Polly.Extensions.Http;

namespace ECommerce.API.Extensions;

public static class MailConfiguration
{
    public static IServiceCollection AddMailServices(this IServiceCollection services, 
                                                     IConfiguration config)
    {
        services.AddHttpClient<IEmailService, MailTrapEmailService>((sp, http) =>
        {
            http.BaseAddress = new Uri("https://send.api.mailtrap.io/");
            http.DefaultRequestHeaders.Add("Api-Token", config["Email:ApiKey"]!);
            http.Timeout = TimeSpan.FromSeconds(30);
        })
        .AddPolicyHandler(HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

        services.AddSingleton(sp =>
            config["Email:FromAddress"] ?? "no-reply@yourapp.dev"
        );

        return services;
    }
}