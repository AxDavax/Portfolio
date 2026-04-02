using ECommerce.Application.DTO;

namespace ECommerce.Application.Interfaces;

public interface IPaymentService
{
    Task<string> CreateCheckoutSessionAsync(OrderHeaderDTO order);
    Task<bool> VerifyPaymentAsync(string sessionId);
}