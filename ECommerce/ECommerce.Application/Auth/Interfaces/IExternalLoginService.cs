using ECommerce.Application.Records;

namespace ECommerce.Application.Auth.Interfaces;

public interface IExternalLoginService
{
    Task<ExternalLoginResult> HandleExternalUserAsync(
        string provider,
        string email,
        string providerUserId);
}