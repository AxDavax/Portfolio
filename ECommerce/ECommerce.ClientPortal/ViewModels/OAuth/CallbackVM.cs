using ECommerce.ClientPortal.Services.API.Implementations;
using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.Services.Extensions;
using ECommerce.ClientPortal.ViewModels.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ECommerce.ClientPortal.ViewModels.OAuth;

public class CallbackVM : ProcessingVM
{
    private readonly CallbackApi _api;
    private readonly NavigationManager _nav;
    private readonly TokenStorageService _tokenStorage;
    private readonly IJSRuntime _js;

    public CallbackVM(CallbackApi api, NavigationManager nav, TokenStorageService tokenStorage, IJSRuntime js)
    {
        _api = api;
        _nav = nav;
        _tokenStorage = tokenStorage;
        _js = js;   
    }

    public async Task InitializedAsync(string provider)
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            try
            {
                var uri = _nav.ToAbsoluteUri(_nav.Uri);
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

                var code = query["code"];
                var state = query["state"];

                if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
                {
                    _nav.NavigateTo("/auth/login?error=invalid_oauth_response", true);
                    return;
                }

                var response = await _api.HandleCallback(provider, code, state);

                if (response.Success)
                {
                    await _tokenStorage.SaveAsync(response.Token, response.RefreshToken, response.Expiration);
                    _nav.NavigateTo("/", true);
                }
                else
                    _nav.NavigateTo("/auth/login?error=oauth_callback_failed", true);
            }
            catch (Exception ex)
            {
                _ = _js.ToastrError($"Unexpected error: {ex.Message}");
            }
        });
    }
}