using ECommerce.Contracts.DTO;

namespace ECommerce.ClientPortal.Services.API.Interfaces;

public interface IProductApi
{
    Task<List<ProductDTO>> GetAllAsync();
    Task<ProductDTO?> GetByIdAsync(int id);
    Task<bool> CreateAsync(ProductDTO dto);
    Task<bool> UpdateAsync(int id, ProductDTO dto);
    Task<bool> DeleteAsync(int id);
}