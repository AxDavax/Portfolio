using ECommerce.Contracts.DTO;

namespace ECommerce.Blazor_WebApp.Client.Services.API.Interfaces;

public interface IPaymentApi
{
    Task<string?> CreateCheckoutSessionAsync(OrderHeaderDTO order);
    Task<bool> VerifyPaymentAsync(string sessionId);
}