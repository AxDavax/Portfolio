using ECommerce.Blazor_WebApp.Client.Services.API.Interfaces;
using ECommerce.Contracts.DTO;
using System.Net.Http.Json;

namespace ECommerce.Blazor_WebApp.Client.Services.API.Implementations
{
    public class CartApi : ICartApi
    {
        private readonly HttpClient _http;

        public CartApi(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<OrderDetailDTO>> ConvertCartToOrderDetailsAsync(List<ShoppingCartDTO> carts)
        {
            var response = await _http.PostAsJsonAsync("api/cart/convert", carts);

            if(!response.IsSuccessStatusCode)
                return new List<OrderDetailDTO>();

            var result = await response.Content.ReadFromJsonAsync<List<OrderDetailDTO>>();
            return result ?? new List<OrderDetailDTO>();
        }
    }
}