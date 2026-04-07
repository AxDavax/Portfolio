using ECommerce.Contracts.DTO;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;
using AutoMapper;

namespace ECommerce.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<CategoryDTO> CreateAsync(CategoryDTO dto)
    {
        var category = _mapper.Map<Category>(dto);
        var created = await _repo.CreateAsync(category);

        return _mapper.Map<CategoryDTO>(created);
    }

    public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);

    public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
    {
        var categories = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
    }

    public async Task<CategoryDTO?> GetByIdAsync(int id)
    {
        var category = await _repo.GetByIdAsync(id);
        return (category == null) ? null : _mapper.Map<CategoryDTO>(category);
    }

    public async Task<bool> UpdateAsync(int id, CategoryDTO dto)
    {
        var category = _mapper.Map<Category>(dto);

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