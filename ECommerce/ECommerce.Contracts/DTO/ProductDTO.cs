using System.ComponentModel.DataAnnotations;

namespace ECommerce.Contracts.DTO;

public class ProductDTO
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Range(0.01, 1000)]
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? SpecialTag { get; set; }
    public string? ImageUrl { get; set; }   


    public int CategoryId { get; set; }
    
    public string CategoryName { get; set; }
}