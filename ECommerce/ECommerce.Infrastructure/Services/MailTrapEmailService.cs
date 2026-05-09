using ECommerce.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ECommerce.Infrastructure.Services;

public class MailTrapEmailService : IEmailService
{
    private readonly HttpClient _http;
    private readonly ILogger<MailTrapEmailService> _logger;
    private readonly string _fromEmail;

    public MailTrapEmailService(HttpClient http, ILogger<MailTrapEmailService> logger, string fromEmail)
    {
        _http = http ?? throw new ArgumentNullException(nameof(http));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        if (string.IsNullOrWhiteSpace(fromEmail))
            throw new ArgumentException("From email cannot be null or empty.", nameof(fromEmail));

        _fromEmail = fromEmail;
    }

    public async Task SendAsync(string to, string subject, string htmlBody)
    {
        try
        {
            var payload = new
            {
                from = new { email = _fromEmail },
                to = new[] { new { email = to } },
                subject,
                html = htmlBody
            };

            _logger.LogInformation("Sending email to {To} with subject: {Subject}", to, subject);

            var response = await _http.PostAsJsonAsync("api/send", payload);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError("Email sending failed: {StatusCode} - {Error}",
                    response.StatusCode, error);
                
                throw new InvalidOperationException($"Failed to send email: {response.StatusCode}");
            }

            _logger.LogInformation("Email sent successfully to {To}", to);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network error while sending email to {To}", to);
            throw;
        }
    }
}