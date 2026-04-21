using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.Contracts.DTO;
using Microsoft.AspNetCore.Components;
namespace ECommerce.ClientPortal.Services.API.Implementations;

public class ProductApi : BaseApi, IProductApi
{
    public ProductApi(HttpClient http, NavigationManager nav) : base(http, nav) { }

    public Task<bool> CreateAsync(ProductDTO dto) => SafePost("api/product", dto);

    public Task<bool> DeleteAsync(int id) => SafeDelete($"api/product/{id}");

    public Task<List<ProductDTO>> GetAllAsync() => SafeGetList<ProductDTO>("api/product");

    public Task<ProductDTO?> GetByIdAsync(int id)
        => SafeGet<ProductDTO?>($"api/product/{id}");

    public Task<bool> UpdateAsync(int id, ProductDTO dto)
        => SafePut($"api/product/{id}", dto);
}