using ECommerce.Contracts.DTO;

namespace ECommerce.Blazor_WebApp.Client.Services.API.Interfaces;

public interface IShoppingCartApi
{
    Task<List<ShoppingCartDTO>> GetAllAsync(string userId);
    Task<ShoppingCartDTO> GetItemAsync(string userId, int productId);
    Task<bool> UpdateAsync(string userId, int productId, int updateBy);
    Task<bool> ClearAsync(string userId);
    Task<int> GetTotalCountAsync(string userId);
}