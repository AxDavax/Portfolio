using ECommerce.Contracts.DTO;

namespace ECommerce.ClientPortal.Services.API.Interfaces;

public interface ICartApi
{
    Task<List<OrderDetailDTO>> ConvertCartToOrderDetailsAsync(List<ShoppingCartDTO> carts);
}