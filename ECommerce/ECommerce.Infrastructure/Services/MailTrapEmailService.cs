using ECommerce.Application.Interfaces;
using System.Net.Http.Json;


namespace ECommerce.Infrastructure.Services;

public class MailTrapEmailService : IEmailService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public MailTrapEmailService(HttpClient http, string apiKey)
    {
        _http = http;
        _apiKey = apiKey;
    }

    public async Task SendAsync(string to, string subject, string htmlBody)
    {
        var payload = new
        {
            from = new { email = "no-reply@yourapp.dev" },
            to = new[] { new { email = to } },
            subject,
            html = htmlBody
        };

        var request = new HttpRequestMessage(HttpMethod.Post,
            "https://send.api.mailtrap.io/api/send")
        {
            Content = JsonContent.Create(payload)
        };

        request.Headers.Add("Api-Token", _apiKey);

        await _http.SendAsync(request);
    }
}