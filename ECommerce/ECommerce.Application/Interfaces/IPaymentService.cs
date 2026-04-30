using ECommerce.Contracts.DTO;
using ECommerce.Contracts.DTO.Payment;

namespace ECommerce.Application.Interfaces;

public interface IPaymentService
{
    Task<CheckoutSessionResponse> CreateCheckoutSessionAsync(OrderHeaderDTO order);
    Task<bool> VerifyPaymentAsync(string sessionId);
}