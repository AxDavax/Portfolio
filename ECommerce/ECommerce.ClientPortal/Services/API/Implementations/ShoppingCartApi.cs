using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.Contracts.DTO;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.Services.API.Implementations;

public class ShoppingCartApi : BaseApi, IShoppingCartApi
{
    public ShoppingCartApi(HttpClient http, NavigationManager nav) : base(http, nav) { }

    public Task<bool> ClearAsync(Guid userId) 
        => SafeDelete($"api/shoppingcart/{userId}");

    public Task<List<ShoppingCartDTO>> GetAllAsync(Guid userId)
        => SafeGetList<ShoppingCartDTO>($"api/shoppingcart/{userId}");

    public Task<ShoppingCartDTO?> GetItemAsync(Guid userId, int productId)
        => SafeGet<ShoppingCartDTO?>($"api/shoppingcart/{userId}/product/{productId}")!;

    public async Task<int> GetTotalCountAsync(Guid userId)
    {
        if (userId == Guid.Empty) return 0;

        var result = await SafeGet<int?>($"api/shoppingcart/{userId}/count");
        return result ?? 0;
    }

    public Task<bool> UpdateAsync(Guid userId, int productId, int updateBy)
    {
        if (userId == Guid.Empty) return Task.FromResult(false);

        return SafePut($"api/shoppingcart/{userId}/product/{productId}?updateBy={updateBy}", null);
    }
}