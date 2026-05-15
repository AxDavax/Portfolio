using ECommerce.Contracts.DTO;
using ECommerce.Domain.Cart.Models;
using ECommerce.Domain.Catalog.Models;

namespace ECommerce.Application.Mappings.Cart;

public static class CartMappings
{
    public static ShoppingCart ToDomain(this ShoppingCartDTO dto)
    {
        return new ShoppingCart
        {
            Id = dto.Id,
            ProductId = dto.ProductId,
            Count = dto.Count,
            Product = new Product
            {
                Id = dto.Product!.Id,
                Name = dto.Product.Name,
                Price = dto.Product.Price
            }
        };
    }
}