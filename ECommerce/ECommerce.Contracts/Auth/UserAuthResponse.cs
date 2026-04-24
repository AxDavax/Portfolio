using ECommerce.Contracts.Interfaces;

namespace ECommerce.Contracts.Auth;

public class UserAuthResponse : AuthResponse, IUserAuth
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
}