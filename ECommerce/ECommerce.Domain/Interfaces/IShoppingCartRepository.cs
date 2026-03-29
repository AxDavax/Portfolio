using ECommerce.Domain.Models;

namespace ECommerce.Domain.Interfaces;

public interface IShoppingCartRepository
{
    Task<ShoppingCart?> GetItemAsync(string userId, int productId);
    public Task<bool> UpdateCartAsync(string userId, int productId, int updateBy);
    public Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId);
    public Task<bool> ClearCartAsync(string? userId);
    public Task<int> GetTotalCartCountAsync(string? userId);
}