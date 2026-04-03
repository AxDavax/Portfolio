using ECommerce.Contracts.DTO;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repo;

    public OrderService(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<OrderHeaderDTO> CreateAsync(OrderHeaderDTO dto)
    {
        var domain = MapToDomain(dto);
        var created = await _repo.CreateAsync(domain);

        return MapToDTO(created);
    }

    public async Task<IEnumerable<OrderHeaderDTO>> GetAllAsync(string? userId = null)
    {
        var orders = await _repo.GetAllAsync(userId);
        return orders.Select(MapToDTO);
    }

    public async Task<OrderHeaderDTO?> GetByIdAsync(int id)
    {
        var order = await _repo.GetByIdAsync(id);
        return order == null ? null : MapToDTO(order);
    }

    public async Task<OrderHeaderDTO?> GetOrderBySessionIdAsync(string sessionId)
    {
        var order = await _repo.GetOrderBySessionIdAsync(sessionId);
        return order == null ? null : MapToDTO(order);
    }

    public async Task<OrderHeaderDTO?> UpdateStatusAsync(int orderId, string status, string? paymentIntentId)
    {
        var updated = await _repo.UpdateStatusAsync(orderId, status, paymentIntentId ?? "");
        return updated == null ? null : MapToDTO(updated);
    }


    private OrderHeaderDTO MapToDTO(OrderHeader header)
    {
        return new OrderHeaderDTO
        {
            Id = header.Id,
            UserId = header.UserId,
            OrderTotal = header.OrderTotal,
            OrderDate = header.OrderDate,
            Status = header.Status,
            Name = header.Name,
            Email = header.Email,
            PhoneNumber = header.PhoneNumber,
            SessionId = header.SessionId,
            PaymentIntentId = header.PaymentIntentId,
            OrderDetails = header.OrderDetails?.Select(d => new OrderDetailDTO
            {
                Id = d.Id,
                OrderHeaderId = d.OrderHeaderId,
                ProductId = d.ProductId,
                Count = d.Count,
                Price = d.Price,
                ProductName = d.ProductName
            }).ToList()!
        };
    }


    private OrderHeader MapToDomain(OrderHeaderDTO dto)
    {
        return new OrderHeader
        {
            Id = dto.Id,
            UserId = dto.UserId,
            OrderTotal = dto.OrderTotal,
            OrderDate = dto.OrderDate,
            Status = dto.Status,
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            SessionId = dto.SessionId,
            PaymentIntentId = dto.PaymentIntentId,
            OrderDetails = dto.OrderDetails?.Select(d => new OrderDetail
            {
                Id = d.Id,
                OrderHeaderId = d.OrderHeaderId,
                ProductId = d.ProductId,
                Count = d.Count,
                Price = d.Price,
                ProductName = d.ProductName!
            }).ToList()!
        };
    }
}