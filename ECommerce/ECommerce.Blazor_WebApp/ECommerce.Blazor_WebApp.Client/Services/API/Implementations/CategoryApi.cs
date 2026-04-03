using ECommerce.Blazor_WebApp.Client.Services.API.Interfaces;
using ECommerce.Contracts.DTO;
using System.Net.Http.Json;

namespace ECommerce.Blazor_WebApp.Client.Services.API.Implementations;

public class CategoryApi : ICategoryApi
{
    private readonly HttpClient _http;

    public CategoryApi(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> CreateAsync(CategoryDTO dto)
    {
        var response = await _http.PostAsJsonAsync("api/category", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/category/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<CategoryDTO>> GetAllAsync()
    {
        var result = await _http.GetFromJsonAsync<List<CategoryDTO>>("api/category");
        return result ?? new List<CategoryDTO>();
    }

    public async Task<CategoryDTO?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<CategoryDTO>($"api/category/{id}");
    }

    public async Task<bool> UpdateAsync(int id, CategoryDTO dto)
    {
        var response = await _http.PutAsJsonAsync($"api/category/{id}", dto);
        return response.IsSuccessStatusCode;
    }
}