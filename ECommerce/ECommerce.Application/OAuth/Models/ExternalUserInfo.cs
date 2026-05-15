namespace ECommerce.Application.OAuth.Models;

public class ExternalUserInfo
{
    public string Email { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string ProviderId { get; set; } = string.Empty;
}