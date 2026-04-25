namespace ECommerce.Contracts.Auth.Refresh;

public class RefreshResponse : UserAuthResponse 
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}