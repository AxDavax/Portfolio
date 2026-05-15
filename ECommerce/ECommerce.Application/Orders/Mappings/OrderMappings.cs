using ECommerce.Contracts.DTO;
using ECommerce.Domain.Orders.Models;

namespace ECommerce.Application.Orders.Mappings;

public static class OrderMappings
{
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