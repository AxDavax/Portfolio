namespace ECommerce.Domain.Models;

public class OrderDetail
{
    public int Id { get; set; }

    public int? OrderHeaderId { get; set; }

    public int ProductId { get; set; }

    public int Count { get; set; }
    
    public double Price { get; set; }
    
    public string ProductName { get; set; }
}