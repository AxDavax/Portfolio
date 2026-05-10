using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Infrastructure.Services;

namespace ECommerce.API.Extensions;

public static class MailConfiguration
{
    public static IServiceCollection AddMailServices(this IServiceCollection services, 
                                                     IConfiguration config)
    {
        services.AddScoped<IEmailService, MailTrapEmailService>();
        services.AddSingleton<IEmailTemplateService, EmailTemplateService>();

        services.AddSingleton(new MailtrapSmtpSettings
        {
            Host = config["Mailtrap:Host"]!,
            Port = int.Parse(config["Mailtrap:Port"]!),
            Username = config["Mailtrap:Username"]!,
            Password = config["Mailtrap:Password"]!,
            FromAddress = config["Mailtrap:FromAddress"]!
        });

        return services;
    }
}