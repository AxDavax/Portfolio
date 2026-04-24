namespace ECommerce.Application.Models;

public class JwtResult
{
    public string Token { get; set; } = default!;
    public DateTime Expiration { get; set; }
}