using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTO;

public class OrderDetailDTO
{
    public int Id { get; set; }
    
    [Required]
    public int Count { get; set; }
        
    [Required]
    public double Price { get; set; }
        
    [Required]
    public string ProductName { get; set; }

    public ProductDTO Product { get; set; }
}