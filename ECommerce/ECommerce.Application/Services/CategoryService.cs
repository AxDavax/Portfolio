using ECommerce.Contracts.DTO;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<CategoryDTO> CreateAsync(CategoryDTO dto)
    {
        var category = new Category { Name = dto.Name };
        var created = await _repo.CreateAsync(category);

        return new CategoryDTO
        {
            Id = created.Id,
            Name = created.Name
        };
    }

    public Task<bool> DeleteAsync(int id)
    {
        return _repo.DeleteAsync(id);
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
    {
        var categories = await _repo.GetAllAsync();
        return categories.Select(c => new CategoryDTO { Id = c.Id, Name = c.Name });
    }

    public async Task<CategoryDTO?> GetByIdAsync(int id)
    {
        var category = await _repo.GetByIdAsync(id);
        if(category == null) return null;

        return new CategoryDTO { Id = category.Id, Name = category.Name };
    }

    public async Task<bool> UpdateAsync(int id, CategoryDTO dto)
    {
        var category = new Category { Id = id, Name = dto.Name };

        try
        {
            await _repo.UpdateAsync(category);
            return true;
        }
        catch(KeyNotFoundException)
        {
            return false;
        }
    }
}