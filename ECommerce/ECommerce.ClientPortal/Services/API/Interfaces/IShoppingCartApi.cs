using ECommerce.Contracts.DTO;

namespace ECommerce.ClientPortal.Services.API.Interfaces;

public interface IShoppingCartApi
{
    Task<List<ShoppingCartDTO>> GetAllAsync(Guid userId);
    Task<ShoppingCartDTO> GetItemAsync(Guid userId, int productId);
    Task<bool> UpdateAsync(Guid userId, int productId, int updateBy);
    Task<bool> ClearAsync(Guid userId);
    Task<int> GetTotalCountAsync(Guid userId);
}