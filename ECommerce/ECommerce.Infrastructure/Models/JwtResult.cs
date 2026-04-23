namespace ECommerce.Infrastructure.Models;

public class JwtResult
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}