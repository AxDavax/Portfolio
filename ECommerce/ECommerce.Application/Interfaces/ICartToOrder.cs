using ECommerce.Domain.Models;

namespace ECommerce.Domain.Interfaces;

public interface ICartToOrder
{
    List<OrderDetail> ConvertShoppingCartToOrderDetails(List<ShoppingCart> carts);
}