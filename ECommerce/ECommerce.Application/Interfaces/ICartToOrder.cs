using ECommerce.Domain.Models;

namespace ECommerce.Application.Interfaces;

public interface ICartToOrder
{
    List<OrderDetail> ConvertShoppingCartToOrderDetails(List<ShoppingCart> carts);
}