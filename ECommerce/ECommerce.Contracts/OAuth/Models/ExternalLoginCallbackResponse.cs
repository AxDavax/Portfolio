using ECommerce.Contracts.Auth;

namespace ECommerce.Contracts.OAuth.Models;

public class ExternalLoginCallbackResponse : AuthResponse
{
    public string? Email { get; set; }
    public string? ProviderUserId { get; set; }
    public string? ErrorMessage { get; set; }
}