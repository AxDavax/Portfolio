using ECommerce.ClientPortal.Services.API.Implementations;
using ECommerce.ClientPortal.Services.Auth;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.ViewModels.OAuth;

public class CallbackVM
{
    private readonly CallbackApi _api;
    private readonly NavigationManager _nav;
    private readonly TokenStorageService _tokenStorage;

    public CallbackVM(CallbackApi api, NavigationManager nav, TokenStorageService tokenStorage)
    {
        _api = api;
        _nav = nav;
        _tokenStorage = tokenStorage;
    }

    public async Task InitializedAsync(string provider, string code, string state)
    {
        var response = await _api.HandleCallback(provider, code, state);
        
        if (response.Success) 
        {
            await _tokenStorage.SaveAsync(response.Token, response.RefreshToken, response.Expiration);
            _nav.NavigateTo("/", true);
        }
        else
            _nav.NavigateTo("/auth/login?error=oauth_callback_failed", true);
    }
}