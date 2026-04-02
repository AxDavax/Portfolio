using ECommerce.Application.DTO;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Mappings;

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

    public static OrderDetailDTO ToDTO(this OrderDetail domain)
    {
        return new OrderDetailDTO
        {
            ProductId = domain.ProductId,
            Count = domain.Count,
            Price = domain.Price,
            ProductName = domain.ProductName
        };
    }
}