using ECommerce.Domain.Models;

namespace ECommerce.Domain.Interfaces;

public interface IOrderService
{
    List<OrderDetail> ConvertShoppingCartToOrderDetails(List<ShoppingCart> carts);
}