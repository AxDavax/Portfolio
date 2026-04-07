using ECommerce.Contracts.DTO;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;
using AutoMapper;

namespace ECommerce.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repo;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<OrderHeaderDTO> CreateAsync(OrderHeaderDTO dto)
    {
        var domain = _mapper.Map<OrderHeader>(dto);
        var created = await _repo.CreateAsync(domain);
        return _mapper.Map<OrderHeaderDTO>(created);
    }

    public async Task<IEnumerable<OrderHeaderDTO>> GetAllAsync(string? userId = null)
    {
        var orders = await _repo.GetAllAsync(userId);
        return _mapper.Map<IEnumerable<OrderHeaderDTO>>(orders);
    }

    public async Task<OrderHeaderDTO?> GetByIdAsync(int id)
    {
        var order = await _repo.GetByIdAsync(id);
        return order == null ? null : _mapper.Map<OrderHeaderDTO>(order);
    }

    public async Task<OrderHeaderDTO?> GetOrderBySessionIdAsync(string sessionId)
    {
        var order = await _repo.GetOrderBySessionIdAsync(sessionId);
        return order == null ? null : _mapper.Map<OrderHeaderDTO>(order);
    }

    public async Task<OrderHeaderDTO?> UpdateStatusAsync(int orderId, string status, string? paymentIntentId)
    {
        var updated = await _repo.UpdateStatusAsync(orderId, status, paymentIntentId ?? "");
        return updated == null ? null : _mapper.Map<OrderHeaderDTO>(updated);
    }
}