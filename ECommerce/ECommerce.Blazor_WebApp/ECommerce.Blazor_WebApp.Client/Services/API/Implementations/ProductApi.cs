using ECommerce.Blazor_WebApp.Client.Services.API.Interfaces;
using ECommerce.Contracts.DTO;
using System.Net.Http.Json;

namespace ECommerce.Blazor_WebApp.Client.Services.API.Implementations;

public class ProductApi : IProductApi
{
    private readonly HttpClient _http;

    public ProductApi(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> CreateAsync(ProductDTO dto)
    {
        var response = await _http.PostAsJsonAsync("api/product", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/product/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<ProductDTO>> GetAllAsync()
    {
        var result = await _http.GetFromJsonAsync<List<ProductDTO>>("api/produc");
        return result ?? new List<ProductDTO>();
    }

    public async Task<ProductDTO?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<ProductDTO>($"api/product/{id}");
    }

    public async Task<bool> UpdateAsync(int id, ProductDTO dto)
    {
        var response = await _http.PutAsJsonAsync($"api/product/{id}", dto);
        return response.IsSuccessStatusCode;
    }
}