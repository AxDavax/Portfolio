using ECommerce.Domain.Cart.Models;

namespace ECommerce.Domain.Cart.Interfaces;

public interface IShoppingCartRepository
{
    Task<ShoppingCart?> GetItemAsync(Guid userId, int productId);
    Task<bool> UpdateCartAsync(Guid userId, int productId, int updateBy);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ShoppingCart>> GetAllAsync(Guid userId);
    Task<bool> ClearCartAsync(Guid userId);
    Task<int> GetTotalCartCountAsync(Guid userId);
}