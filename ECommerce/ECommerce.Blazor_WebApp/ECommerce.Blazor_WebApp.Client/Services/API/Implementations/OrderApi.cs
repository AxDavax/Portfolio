using ECommerce.Blazor_WebApp.Client.Services.API.Interfaces;
using ECommerce.Contracts.DTO;
using System.Net.Http.Json;

namespace ECommerce.Blazor_WebApp.Client.Services.API.Implementations;

public class OrderApi : IOrderApi
{
    private readonly HttpClient _http;

    public OrderApi(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> CreateAsync(OrderHeaderDTO dto)
    {
        var response = await _http.PostAsJsonAsync("api/order", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<OrderHeaderDTO>> GetAllAsync(string? userId = null)
    {
        string url = userId is null
            ? "api/order"
            : $"api/order?userId={userId}";

        var result = await _http.GetFromJsonAsync<List<OrderHeaderDTO>>(url);
        return result ?? new List<OrderHeaderDTO>();
    }

    public async Task<OrderHeaderDTO?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<OrderHeaderDTO>($"api/order/{id}");
    }

    public async Task<OrderHeaderDTO?> GetBySessionIdAsync(string sessionId)
    {
        return await _http.GetFromJsonAsync<OrderHeaderDTO>($"api/order/session/{sessionId}");
    }

    public async Task<bool> UpdateAsync(int orderId, string status, string? paymentIntentId = null)
    {
        string url = $"api/order/{orderId}/status?status={status}";

        if (!string.IsNullOrWhiteSpace(paymentIntentId))
            url += $"&paymentIntentId={paymentIntentId}";

        var response = await _http.PutAsync(url, null);
        return response.IsSuccessStatusCode;
    }
}