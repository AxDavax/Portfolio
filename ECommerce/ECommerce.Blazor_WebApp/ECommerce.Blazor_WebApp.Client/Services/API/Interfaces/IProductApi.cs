using ECommerce.Contracts.DTO;

namespace ECommerce.Blazor_WebApp.Client.Services.API.Interfaces;

public interface IProductApi
{
    Task<List<ProductDTO>> GetAllAsync();
    Task<ProductDTO?> GetByIdAsync(int id);
    Task<bool> CreateAsync(ProductDTO dto);
    Task<bool> UpdateAsync(int id, ProductDTO dto);
    Task<bool> DeleteAsync(int id);
}