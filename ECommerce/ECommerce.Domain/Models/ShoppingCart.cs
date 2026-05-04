namespace ECommerce.Domain.Models;

public class ShoppingCart
{
    public int Id { get; set; } 
    
    public Guid UserId { get; set; }

    public int Count { get; set; }


    public int ProductId { get; set; }

    public Product? Product { get; set; }
}