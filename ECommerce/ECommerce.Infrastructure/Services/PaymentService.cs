using ECommerce.Domain.Constants;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;
using Microsoft.Extensions.Configuration;
using Stripe.Checkout;

namespace ECommerce.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IOrderRepository _orderRepository;
    private readonly string _successUrl;
    private readonly string _cancelUrl;

    public PaymentService(IOrderRepository orderRepository, IConfiguration config)
    {
        _orderRepository = orderRepository;

        _successUrl = config["Stripe:SuccessUrl"]!;
        _cancelUrl = config["Stripe:CancelUrl"]!;
    }

    public async Task<string> CreateCheckoutSessionAsync(OrderHeader orderHeader)
    {
        var lineItems = orderHeader.OrderDetails
            .Select(order => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "cad",
                    UnitAmountDecimal = (decimal?)order.Price * 100,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = order.ProductName,
                    }
                },
                Quantity = order.Count
            }).ToList();

        var options = new SessionCreateOptions
        {
            SuccessUrl = $"{_successUrl}/{orderHeader.Id}",
            CancelUrl = _cancelUrl,
            LineItems = lineItems,
            Mode = "payment"
        };

        var service = new SessionService();
        var session = service.Create(options);

        await _orderRepository.UpdateStatusAsync(orderHeader.Id, OrderStatus.StatusPending, session.Id);
        return session.Url;
    }

    public async Task<bool> VerifyPaymentAsync(string sessionId)
    {
        var service = new SessionService();
        var session = service.Get(sessionId);

        if (session.PaymentStatus?.ToLower() == "paid")
        {
            OrderHeader? orderHeader = await _orderRepository.GetOrderBySessionIdAsync(sessionId);

            await _orderRepository.UpdateStatusAsync(orderHeader!.Id, OrderStatus.StatusApproved, session.PaymentIntentId);
            return true;
        }

        return false;
    }
}