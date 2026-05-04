using ECommerce.Contracts.DTO;

namespace ECommerce.Application.Interfaces;

public interface IShoppingCartService
{
    Task<ShoppingCartDTO?> GetItemAsync(Guid userId, int productId);
    Task<IEnumerable<ShoppingCartDTO>> GetAllAsync(Guid userId);
    Task<bool> UpdateCartAsync(Guid userId, int productId, int updateBy);
    Task<bool> ClearCartAsync(Guid userId);
    Task<int> GetTotalCartCountAsync(Guid userId);
}