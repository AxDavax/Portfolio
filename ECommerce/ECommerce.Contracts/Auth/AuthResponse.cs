namespace ECommerce.Contracts.Auth;

public abstract class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }

    public bool Success { get; set; }
    public string? Message { get; set; }
}