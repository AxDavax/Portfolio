using ECommerce.Contracts.DTO;

namespace ECommerce.Blazor_WebApp.Client.Services.API.Interfaces;

public interface ICartApi
{
    Task<List<OrderDetailDTO>> ConvertCartToOrderDetailsAsync(List<ShoppingCartDTO> carts);
}