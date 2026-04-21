using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.Contracts.DTO;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.Services.API.Implementations;

public class CartApi : BaseApi, ICartApi
{
    public CartApi(HttpClient http, NavigationManager nav) : base(http, nav) { }

    public async Task<List<OrderDetailDTO>> ConvertCartToOrderDetailsAsync(List<ShoppingCartDTO> carts)
    {
        var result = await SafePost<List<OrderDetailDTO>>("api/cart/convert", carts);
        return result ?? new List<OrderDetailDTO>();
    }
}