using Microsoft.AspNetCore.Components;
using Portfolio.ECommerce.Blazor.Data;
using Portfolio.ECommerce.Blazor.Repository.IRepository;
using Portfolio.ECommerce.Blazor.Utility;
using Stripe;
using Stripe.Checkout;

namespace Portfolio.ECommerce.Blazor.Services
{
    public class PaymentService
    {
        private readonly NavigationManager _navigationManager;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(NavigationManager navigationManager, IOrderRepository orderRepository)
        {
            _navigationManager = navigationManager;
            _orderRepository = orderRepository;
        }

        public Session CreateStripeCheckoutSession(OrderHeader orderHeader)
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
                SuccessUrl = $"{_navigationManager.BaseUri}order/confirmation/{{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_navigationManager.BaseUri}cart",
                LineItems = lineItems,
                Mode = "payment",
            };

            return new SessionService().Create(options);
        }

        public async Task<OrderHeader> CheckPaymentStatusAndUpdateOrder(string sessionId)
        {
            OrderHeader orderHeader = await _orderRepository.GetOrderBySessionIdAsync(sessionId);
            var service = new SessionService();
            var session = service.Get(sessionId);
            if(session.PaymentStatus.ToLower() == "paid")
            {
                await _orderRepository.UpdateStatusAsync(orderHeader.Id, SD.StatusApproved, session.PaymentIntentId);
            }

            return orderHeader;
        }
    }
}