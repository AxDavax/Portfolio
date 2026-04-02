using ECommerce.Domain.Models;

namespace ECommerce.Domain.Interfaces;

public interface IShoppingCartRepository
{
    Task<ShoppingCart?> GetItemAsync(string userId, int productId);
    Task<bool> UpdateCartAsync(string userId, int productId, int updateBy);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId);
    Task<bool> ClearCartAsync(string? userId);
    Task<int> GetTotalCartCountAsync(string? userId);
}