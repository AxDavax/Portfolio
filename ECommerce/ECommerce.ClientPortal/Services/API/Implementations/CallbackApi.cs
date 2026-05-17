using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.Contracts.OAuth.Records;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.Services.API.Implementations
{
    public class CallbackApi : BaseApi, ICallbackApi
    {
        public CallbackApi(HttpClient http, NavigationManager nav) : base(http, nav) { }

        public Task<ExternalLoginCallbackResponse?> HandleCallback(string provider, string code, string state)
            => SafeGet<ExternalLoginCallbackResponse>(
                $"api/oauth/external/{provider}/callback?code={code}&state={state}");
    }
}