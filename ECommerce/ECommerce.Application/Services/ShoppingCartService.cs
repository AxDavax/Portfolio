using ECommerce.Application.DTO;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _repo;
    private readonly IProductRepository _productRepo;

    public ShoppingCartService(
        IShoppingCartRepository repo,
        IProductRepository productRepo)
    {
        _repo = repo;
        _productRepo = productRepo;
    }

    public async Task<bool> ClearCartAsync(string userId)
    {
        return await _repo.ClearCartAsync(userId);
    }

    public async Task<IEnumerable<ShoppingCartDTO>> GetAllAsync(string userId)
    {
        var items = await _repo.GetAllAsync(userId);
        return items.Select(MapToDTO);
    }

    public async Task<ShoppingCartDTO?> GetItemAsync(string userId, int productId)
    {
        var cartItem = await _repo.GetItemAsync(userId, productId);
        if (cartItem == null) return null;

        return MapToDTO(cartItem);
    }

    public async Task<int> GetTotalCartCountAsync(string userId)
    {
        return await _repo.GetTotalCartCountAsync(userId);
    }

    public async Task<bool> UpdateCartAsync(string userId, int productId, int updateBy)
    {
        var existing = await _repo.GetItemAsync(userId, productId);

        if (existing != null && existing.Count + updateBy <= 0)
            return await _repo.DeleteAsync(existing.Id);

        if (existing == null && updateBy > 0)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
                return false;
        }

        return await _repo.UpdateCartAsync(userId, productId, updateBy);
    }


    private ShoppingCartDTO MapToDTO(ShoppingCart cart)
    {
        return new ShoppingCartDTO
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Count = cart.Count,
            ProductId = cart.ProductId,
            Product = cart.Product == null ? null : new ProductDTO
            {
                Id = cart.Product.Id,
                Name = cart.Product.Name,
                Price = cart.Product.Price,
                Description = cart.Product.Description,
                SpecialTag = cart.Product.SpecialTag,
                ImageUrl = cart.Product.ImageUrl,
                CategoryId = cart.Product.CategoryId,
                CategoryName = cart.Product.Category?.Name!
            }
        };
    }
}