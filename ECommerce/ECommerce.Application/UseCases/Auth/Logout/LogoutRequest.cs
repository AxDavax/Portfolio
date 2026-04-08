namespace ECommerce.Application.UseCases.Auth.Logout;

public class LogoutRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}