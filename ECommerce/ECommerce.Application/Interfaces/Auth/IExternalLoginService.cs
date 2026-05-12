using ECommerce.Application.Records;

namespace ECommerce.Application.Interfaces.Auth;

public interface IExternalLoginService
{
    Task<ExternalLoginResult> HandleExternalUserAsync(
        string provider,
        string email,
        string providerUserId);
}