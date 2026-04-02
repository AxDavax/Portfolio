using ECommerce.Application.DTO;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Constants;
using ECommerce.Domain.Interfaces;
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

    public async Task<string> CreateCheckoutSessionAsync(OrderHeaderDTO orderHeader)
    {
        if (orderHeader.OrderDetails == null || !orderHeader.OrderDetails.Any())
            throw new InvalidOperationException("Order must contain at least one item.");

        var lineItems = orderHeader.OrderDetails
            .Select(order => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "cad",
                    UnitAmountDecimal = (decimal?)order.Price * 100,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = order.ProductName ?? "Product"
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
        var session = await service.CreateAsync(options);

        await _orderRepository.UpdateStatusAsync(orderHeader.Id, OrderStatus.StatusPending, session.Id);
        
        return session.Url;
    }

    public async Task<bool> VerifyPaymentAsync(string sessionId)
    {
        var service = new SessionService();
        var session = await service.GetAsync(sessionId);

        if (session.PaymentStatus?.ToLower() == "paid")
        {
            var orderHeader = await _orderRepository.GetOrderBySessionIdAsync(sessionId);
            if (orderHeader == null)
                return false;

            await _orderRepository.UpdateStatusAsync(
                orderHeader!.Id, 
                OrderStatus.StatusApproved, 
                session.PaymentIntentId
            );
            
            return true;
        }

        return false;
    }
}