using ECommerce.Contracts.OAuth.Models;

namespace ECommerce.ClientPortal.Services.API.Interfaces;

public interface ICallbackApi
{
    Task<ExternalLoginCallbackResponse?> HandleCallback(string provider, string code, string state);
}