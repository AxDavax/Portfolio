using ECommerce.Domain.Cart.Models;
using ECommerce.Domain.Orders.Models;

namespace ECommerce.Application.Orders.Interfaces;

public interface ICartToOrder
{
    List<OrderDetail> ConvertShoppingCartToOrderDetails(List<ShoppingCart> carts);
}