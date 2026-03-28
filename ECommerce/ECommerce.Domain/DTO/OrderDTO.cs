namespace ECommerce.Domain.DTO;

public class OrderDTO
{
    public OrderHeaderDTO Header { get; set; }
    public List<OrderDetailDTO> Details { get; set; } = new();
}