namespace ECommerce.Domain.Models;

public class OrderHeader
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public double OrderTotal { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; }

    public string Name { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public string? SessionId { get; set; }

    public string? PaymentIntentId { get; set; }

    public List<OrderDetail> OrderDetails { get; set; } = new();
}