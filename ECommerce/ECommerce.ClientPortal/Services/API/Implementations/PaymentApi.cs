using ECommerce.ClientPortal.Models;
using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.Contracts.DTO;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.Services.API.Implementations;

public class PaymentApi : BaseApi, IPaymentApi
{
    public PaymentApi(HttpClient http, NavigationManager nav) : base(http, nav) { }

    public Task<string?> CreateCheckoutSessionAsync(OrderHeaderDTO order)
        => SafePostRaw("api/payment/create-session", order);

    public async Task<bool> VerifyPaymentAsync(string sessionId)
    {
        var result = await SafeGet<PaymentVerifyResponse>($"api/payment/verify/{sessionId}");
        return result?.Success ?? false;
    }
}