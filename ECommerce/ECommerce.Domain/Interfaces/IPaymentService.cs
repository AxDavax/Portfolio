using ECommerce.Domain.Models;

namespace ECommerce.Domain.Interfaces;

public interface IPaymentService
{
    Task<string> CreateCheckoutSessionAsync(OrderHeader order);
    Task<bool> VerifyPaymentAsync(string sessionId);
}