using ECommerce.Application.Models;

namespace ECommerce.Application.OAuth.Interfaces;

public interface IExternalAuthService
{
    string GetAuthorizationUrl(string state);
    Task<ExternalUserInfo?> GetUserInfoAsync(string code);
}