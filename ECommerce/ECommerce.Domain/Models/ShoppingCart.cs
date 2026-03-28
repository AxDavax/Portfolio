namespace ECommerce.Domain.Models;

public class ShoppingCart
{
    public int Id { get; set; } 
    
    public string UserId { get; set; }

    public int ProductId { get; set; }

    public int Count { get; set; }  
}