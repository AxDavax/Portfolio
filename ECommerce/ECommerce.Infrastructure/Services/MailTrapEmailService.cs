using ECommerce.Application.Email.Interfaces;
using ECommerce.Application.Email.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace ECommerce.Infrastructure.Services;

public class MailTrapEmailService : IEmailService
{
    private readonly MailtrapSmtpSettings _settings;
    private readonly ILogger<MailTrapEmailService> _logger;

    public MailTrapEmailService(
        MailtrapSmtpSettings settings,
        ILogger<MailTrapEmailService> logger)
    {
        _settings = settings;
        _logger = logger;
    }

    public async Task SendAsync(string to, string subject, string htmlBody)
    {
        try
        {
            using var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_settings.FromAddress),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            mail.To.Add(to);

            _logger.LogInformation("Sending SMTP email to {To}", to);

            await client.SendMailAsync(mail);

            _logger.LogInformation("Email sent successfully to {To}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SMTP email to {To}", to);
            throw;
        }
    }
}