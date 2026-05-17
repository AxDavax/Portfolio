using ECommerce.Contracts.OAuth.Records;

namespace ECommerce.ClientPortal.Services.API.Interfaces;

public interface ICallbackApi
{
    Task<ExternalLoginCallbackResponse?> HandleCallback(string provider, string code, string state);
}