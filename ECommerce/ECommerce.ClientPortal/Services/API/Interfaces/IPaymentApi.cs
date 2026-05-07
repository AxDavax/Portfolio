using ECommerce.Contracts.DTO;
using ECommerce.Contracts.DTO.Payment;

namespace ECommerce.ClientPortal.Services.API.Interfaces;

public interface IPaymentApi
{
    Task<CheckoutSessionResponse?> CreateCheckoutSessionAsync(OrderHeaderDTO order);
    Task<PaymentVerifyResponse?> VerifyPaymentAsync(string sessionId);
}