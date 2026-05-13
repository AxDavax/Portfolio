using ECommerce.Domain.Catalog.Models;

namespace ECommerce.Domain.Cart.Models;

public class ShoppingCart
{
    public int Id { get; set; } 
    
    public Guid UserId { get; set; }

    public int Count { get; set; }


    public int ProductId { get; set; }

    public Product? Product { get; set; }
}