using ECommerce.Contracts.DTO;

namespace ECommerce.Application.Interfaces;

public interface IShoppingCartService
{
    Task<ShoppingCartDTO?> GetItemAsync(string userId, int productId);
    Task<IEnumerable<ShoppingCartDTO>> GetAllAsync(string userId);
    Task<bool> UpdateCartAsync(string userId, int productId, int updateBy);
    Task<bool> ClearCartAsync(string userId);
    Task<int> GetTotalCartCountAsync(string userId);
}