namespace ECommerce.Domain.Auth.Models;

public class RefreshToken
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }

    public bool IsExpired => ExpiryDate <= DateTime.UtcNow;
}