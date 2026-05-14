using ECommerce.Contracts.DTO;

namespace ECommerce.Application.Orders.Interfaces;

public interface IOrderService
{
    Task<OrderHeaderDTO> CreateAsync(OrderHeaderDTO dto);
    Task<IEnumerable<OrderHeaderDTO>> GetAllAsync(Guid? userId = null);
    Task<OrderHeaderDTO?> GetByIdAsync(int id);
    Task<OrderHeaderDTO?> GetOrderBySessionIdAsync(string sessionId);
    Task<OrderHeaderDTO?> UpdateStatusAsync(int orderId, string status, string? paymentIntentId);
}