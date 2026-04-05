using ECommerce.Contracts.DTO;

namespace ECommerce.ClientPortal.Services.API.Interfaces;

public interface IOrderApi
{
    Task<List<OrderHeaderDTO>> GetAllAsync(string? userId = null);
    Task<OrderHeaderDTO?> GetByIdAsync(int id);
    Task<OrderHeaderDTO?> GetBySessionIdAsync(string sessionId);
    Task<bool> CreateAsync(OrderHeaderDTO dto);
    Task<bool> UpdateAsync(int orderId, string status, string? paymentIntentId = null); 
}