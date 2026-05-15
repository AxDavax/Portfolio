using ECommerce.Application.Records;

namespace ECommerce.Application.OAuth.Interfaces;

public interface IExternalLoginService
{
    Task<ExternalLoginResult> HandleExternalUserAsync(
        string provider,
        string email,
        string providerUserId);
}