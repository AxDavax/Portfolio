using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.Contracts.DTO;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.Services.API.Implementations;

public class ShoppingCartApi : BaseApi, IShoppingCartApi
{
    public ShoppingCartApi(HttpClient http, NavigationManager nav) : base(http, nav) { }

    public Task<bool> ClearAsync(string userId) 
        => SafeDelete($"api/shoppingcart/{userId}");

    public async Task<List<ShoppingCartDTO>> GetAllAsync(string userId)
    {
        var result = await SafeGet<List<ShoppingCartDTO>>($"api/shoppingcart/{userId}");
        return result ?? new List<ShoppingCartDTO>();
    }

    public Task<ShoppingCartDTO?> GetItemAsync(string userId, int productId)
        => SafeGet<ShoppingCartDTO?>($"api/shoppingcart/{userId}/product/{productId}")!;

    public async Task<int> GetTotalCountAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId)) return 0;

        var result = await SafeGet<int?>($"api/shoppingcart/{userId}/count");
        return result ?? 0;
    }

    public Task<bool> UpdateAsync(string userId, int productId, int updateBy)
    {
        if (string.IsNullOrEmpty(userId)) return Task.FromResult(false);

        return SafePut($"api/shoppingcart/{userId}/product/{productId}?updateBy={updateBy}", null);
    }
}