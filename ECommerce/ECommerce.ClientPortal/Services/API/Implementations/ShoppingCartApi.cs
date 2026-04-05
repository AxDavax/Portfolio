using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.Contracts.DTO;
using System.Net.Http.Json;

namespace ECommerce.ClientPortal.Services.API.Implementations;

public class ShoppingCartApi : IShoppingCartApi
{
    private readonly HttpClient _http;

    public ShoppingCartApi(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> ClearAsync(string userId)
    {
        var response = await _http.DeleteAsync($"api/shoppingcart/{userId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<ShoppingCartDTO>> GetAllAsync(string userId)
    {
        var result = await _http.GetFromJsonAsync<List<ShoppingCartDTO>>(
            $"api/shoppingcart/{userId}");

        return result ?? new List<ShoppingCartDTO>();
    }

    public async Task<ShoppingCartDTO?> GetItemAsync(string userId, int productId)
    {
        return await _http.GetFromJsonAsync<ShoppingCartDTO>(
            $"api/shoppingcart/{userId}/product/{productId}");
    }

    public async Task<int> GetTotalCountAsync(string userId)
    {
        var result = await _http.GetFromJsonAsync<int>(
            $"api/shoppingcart/{userId}/count");

        return result;
    }

    public async Task<bool> UpdateAsync(string userId, int productId, int updateBy)
    {
        var response = await _http.PutAsync(
            $"api/shoppingcart/{userId}/product/{productId}?updateBy={updateBy}", null);

        return response.IsSuccessStatusCode;
    }
}