using ECommerce.Contracts.DTO;

namespace ECommerce.ClientPortal.Services.API.Interfaces;

public interface IPaymentApi
{
    Task<string?> CreateCheckoutSessionAsync(OrderHeaderDTO order);
    Task<bool> VerifyPaymentAsync(string sessionId);
}