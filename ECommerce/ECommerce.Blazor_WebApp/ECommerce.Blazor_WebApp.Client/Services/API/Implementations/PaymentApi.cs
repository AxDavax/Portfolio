using ECommerce.Blazor_WebApp.Client.Services.API.Interfaces;
using ECommerce.Contracts.DTO;
using System.Net.Http.Json;

namespace ECommerce.Blazor_WebApp.Client.Services.API.Implementations;

public class PaymentApi : IPaymentApi
{
    private readonly HttpClient _http;

    public PaymentApi(HttpClient http)
    {
        _http = http;
    }

    public async Task<string?> CreateCheckoutSessionAsync(OrderHeaderDTO order)
    {
        var response = await _http.PostAsJsonAsync("api/payment/create-session", order);

        if(!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<bool> VerifyPaymentAsync(string sessionId)
    {
        return await _http.GetFromJsonAsync<bool>($"api/payment/verify/{sessionId}");
    }
}