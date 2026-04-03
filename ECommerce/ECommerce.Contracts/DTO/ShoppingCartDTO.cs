using System.ComponentModel.DataAnnotations;

namespace ECommerce.Contracts.DTO;

public class ShoppingCartDTO
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [Range(1, 100)]
    public int Count { get; set; }

    public int ProductId { get; set; }

    public ProductDTO? Product { get; set; }
}