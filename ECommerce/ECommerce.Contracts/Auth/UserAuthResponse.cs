namespace ECommerce.Contracts.Auth;

public class UserAuthResponse : AuthResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
}