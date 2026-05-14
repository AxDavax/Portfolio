using ECommerce.Application.Models;

namespace ECommerce.Application.Auth.Interfaces;

public interface IExternalAuthService
{
    string GetAuthorizationUrl(string state);
    Task<ExternalUserInfo?> GetUserInfoAsync(string code);
}