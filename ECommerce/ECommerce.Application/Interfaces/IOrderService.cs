using ECommerce.Contracts.DTO;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderHeaderDTO> CreateAsync(OrderHeaderDTO dto);
        Task<IEnumerable<OrderHeaderDTO>> GetAllAsync(string? userId = null);
        Task<OrderHeaderDTO?> GetByIdAsync(int id);
        Task<OrderHeaderDTO?> GetOrderBySessionIdAsync(string sessionId);
        Task<OrderHeaderDTO?> UpdateStatusAsync(int orderId, string status, string? paymentIntentId);
    }
}