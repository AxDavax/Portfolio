using ECommerce.Contracts.DTO;

namespace ECommerce.Blazor_WebApp.Client.Services.API.Interfaces;

public interface ICategoryApi
{
    Task<List<CategoryDTO>> GetAllAsync();
    Task<CategoryDTO?> GetByIdAsync(int id);
    Task<bool> CreateAsync(CategoryDTO dto);
    Task<bool> UpdateAsync(int id, CategoryDTO dto);
    Task<bool> DeleteAsync(int id);
}