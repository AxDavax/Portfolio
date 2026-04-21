using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.Contracts.DTO;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.Services.API.Implementations;

public class CategoryApi : BaseApi, ICategoryApi
{
    public CategoryApi(HttpClient http, NavigationManager nav) : base(http, nav) { }

    public Task<bool> CreateAsync(CategoryDTO dto) => SafePost("api/category", dto);

    public Task<bool> DeleteAsync(int id) => SafeDelete($"api/category/{id}");

    public async Task<List<CategoryDTO>> GetAllAsync()
    {
        var result = await SafeGet<List<CategoryDTO>>("api/category");
        return result ?? new List<CategoryDTO>();
    }

    public Task<CategoryDTO?> GetByIdAsync(int id) 
        => SafeGet<CategoryDTO?>($"api/category/{id}");

    public Task<bool> UpdateAsync(int id, CategoryDTO dto) 
        => SafePut($"api/category/{id}", dto);
}