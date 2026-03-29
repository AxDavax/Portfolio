using ECommerce.Domain.Models;

namespace ECommerce.Domain.Interfaces;

public interface IOrderRepository
{
    public Task<OrderHeader> CreateAsync(OrderHeader orderHeader);
    public Task<OrderHeader?> GetByIdAsync(int id);
    public Task<OrderHeader?> GetOrderBySessionIdAsync(string sessionId);
    public Task<IEnumerable<OrderHeader>> GetAllAsync(string? userId = null);
    public Task<OrderHeader?> UpdateStatusAsync(int orderId, string status, string? paymentIntentId);
}