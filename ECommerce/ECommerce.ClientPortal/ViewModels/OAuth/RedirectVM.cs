using ECommerce.ClientPortal.Services.API.Implementations;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.ViewModels.OAuth;

public class RedirectVM
{
    private readonly RedirectApi _api;
    private readonly NavigationManager _nav;

    public RedirectVM(RedirectApi api, NavigationManager nav)
    {
        _api = api;
        _nav = nav;
    }   

    private async Task RedirectAsync(string provider)
    {
        var url = await _api.GetOAuthRedirectUrlAsync(provider);
        
        if (!string.IsNullOrWhiteSpace(url))
            _nav.NavigateTo(url, true);
        else
            _nav.NavigateTo("/auth/login?error=oauth_redirect_failed", true);
    }

    public Task InitializedAsync(string provider) => RedirectAsync(provider);

    public Task StartAsync(string provider) => RedirectAsync(provider);
}