using ECommerce.Contracts.DTO;
using ECommerce.Contracts.DTO.Payment;

namespace ECommerce.Application.Payments.Interfaces;

public interface IPaymentService
{
    Task<CheckoutSessionResponse> CreateCheckoutSessionAsync(OrderHeaderDTO order);
    Task<OrderHeaderDTO> VerifyPaymentAsync(string sessionId);
}