using ECommerce.Contracts.DTO;
using AutoMapper;
using ECommerce.Domain.Catalog.Interfaces;
using ECommerce.Domain.Cart.Interfaces;
using ECommerce.Application.Cart.Interfaces;

namespace ECommerce.Application.Cart.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _repo;
    private readonly IProductRepository _productRepo;
    private readonly IMapper _mapper;

    public ShoppingCartService(
        IShoppingCartRepository repo,
        IProductRepository productRepo,
        IMapper mapper)
    {
        _repo = repo;
        _productRepo = productRepo;
        _mapper = mapper;
    }

    public async Task<bool> ClearCartAsync(Guid userId) => await _repo.ClearCartAsync(userId);

    public async Task<IEnumerable<ShoppingCartDTO>> GetAllAsync(Guid userId)
    {
        var items = await _repo.GetAllAsync(userId);
        return _mapper.Map<IEnumerable<ShoppingCartDTO>>(items);
    }

    public async Task<ShoppingCartDTO?> GetItemAsync(Guid userId, int productId)
    {
        var cartItem = await _repo.GetItemAsync(userId, productId);
        return cartItem == null ? null : _mapper.Map<ShoppingCartDTO>(cartItem);
    }

    public async Task<int> GetTotalCartCountAsync(Guid userId)
        => await _repo.GetTotalCartCountAsync(userId);

    public async Task<bool> UpdateCartAsync(Guid userId, int productId, int updateBy)
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
}