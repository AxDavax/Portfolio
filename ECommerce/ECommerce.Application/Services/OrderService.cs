using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Services;

public class OrderService : IOrderService
{
    public List<OrderDetail> ConvertShoppingCartToOrderDetails(List<ShoppingCart> carts)
    {
        return carts.Select(cart => new OrderDetail
        {
            ProductId = cart.ProductId,
            Count = cart.Count,
            Price = Convert.ToDouble(cart.Product.Price),
            ProductName = cart.Product.Name
        }).ToList();
    }
}