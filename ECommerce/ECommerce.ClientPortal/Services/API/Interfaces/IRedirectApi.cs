namespace ECommerce.ClientPortal.Services.API.Interfaces;

public interface IRedirectApi
{
    Task<string?> GetOAuthRedirectUrlAsync(string provider);
}