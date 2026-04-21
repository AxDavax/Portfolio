using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.Contracts.DTO;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.Services.API.Implementations;

public class OrderApi : BaseApi, IOrderApi
{
    public OrderApi(HttpClient http, NavigationManager nav) : base(http, nav) { }

    public Task<bool> CreateAsync(OrderHeaderDTO dto) => SafePost("api/order", dto);

    public async Task<List<OrderHeaderDTO>> GetAllAsync(string? userId = null)
    {
        string url = userId is null ? "api/order" : $"api/order?userId={userId}";
        var result = await SafeGet<List<OrderHeaderDTO>>(url);
        return result ?? new List<OrderHeaderDTO>();
    }

    public Task<OrderHeaderDTO?> GetByIdAsync(int id)
        => SafeGet<OrderHeaderDTO?>($"api/order/{id}");

    public Task<OrderHeaderDTO?> GetBySessionIdAsync(string sessionId)
        => SafeGet<OrderHeaderDTO?>($"api/order/session/{sessionId}");

    public Task<bool> UpdateAsync(int orderId, string status, string? paymentIntentId = null)
    {
        string url = $"api/order/{orderId}/status?status={status}";

        if (!string.IsNullOrWhiteSpace(paymentIntentId))
            url += $"&paymentIntentId={paymentIntentId}";

        return SafePut(url, null);
    }
}