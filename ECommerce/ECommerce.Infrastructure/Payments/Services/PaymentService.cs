using AutoMapper;
using ECommerce.Application.Payments.Interfaces;
using ECommerce.Contracts.DTO;
using ECommerce.Contracts.DTO.Payment;
using ECommerce.Domain.Cart.Interfaces;
using ECommerce.Domain.Constants;
using ECommerce.Domain.Orders.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe.Checkout;

namespace ECommerce.Infrastructure.Payments.Services;

public class PaymentService : IPaymentService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IShoppingCartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly string _successUrl;
    private readonly string _cancelUrl;

    public PaymentService(IOrderRepository orderRepository, 
                          IShoppingCartRepository cartRepository, 
                          IMapper mapper,
                          IConfiguration config)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;   
        _mapper = mapper;

        _successUrl = config["Stripe:SuccessUrl"]!;
        _cancelUrl = config["Stripe:CancelUrl"]!;
    }

    public async Task<CheckoutSessionResponse> CreateCheckoutSessionAsync(OrderHeaderDTO orderHeader)
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
            SuccessUrl = $"{_successUrl}?session_id={{CHECKOUT_SESSION_ID}}",
            CancelUrl = _cancelUrl,
            LineItems = lineItems,
            Mode = "payment"
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);
        
        await _orderRepository.UpdateStatusAsync(orderHeader.Id, OrderStatus.StatusPending, session.Id);
        
        return new CheckoutSessionResponse
        {
            Id = session.Id,
            Url = session.Url
        };
    }

    public async Task<OrderHeaderDTO> VerifyPaymentAsync(string sessionId)
    {
        var service = new SessionService();
        var session = await service.GetAsync(sessionId);

        if (session.PaymentStatus?.ToLower() != "paid")
            return null;

        var orderHeader = await _orderRepository.GetOrderBySessionIdAsync(sessionId);
        if (orderHeader == null)
            return null;

        var newOrderHeader = await _orderRepository.UpdateStatusAsync(
            orderHeader!.Id, 
            OrderStatus.StatusApproved, 
            session.PaymentIntentId
        );
            
        await _cartRepository.ClearCartAsync(orderHeader.UserId);

        return _mapper.Map<OrderHeaderDTO>(newOrderHeader);
    }
}