using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.ClientPortal.Services.Extensions;
using ECommerce.ClientPortal.ViewModels.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ECommerce.ClientPortal.ViewModels.OAuth;

public class RedirectVM : ProcessingVM
{
    private readonly IRedirectApi _api;
    private readonly NavigationManager _nav;
    private readonly IJSRuntime _js;

    public RedirectVM(IRedirectApi api, NavigationManager nav, IJSRuntime js)
    {
        _api = api;
        _nav = nav;
        _js = js;
    }   

    private async Task RedirectAsync(string provider)
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            try
            {
                var url = await _api.GetOAuthRedirectUrlAsync(provider);

                if (!string.IsNullOrWhiteSpace(url))
                    _nav.NavigateTo(url, true);
                else
                    _nav.NavigateTo("/auth/login?error=oauth_redirect_failed", true);
            }
            catch (Exception ex)
            {
                _ = _js.ToastrError($"Unexpected error: {ex.Message}");
            }
        });
    }

    public Task InitializedAsync(string provider) => RedirectAsync(provider);

    public Task StartAsync(string provider) => RedirectAsync(provider);
}