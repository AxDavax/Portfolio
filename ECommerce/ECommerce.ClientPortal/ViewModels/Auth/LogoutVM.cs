using ECommerce.ClientPortal.Providers;
using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.ViewModels.Core;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.ViewModels.Auth;

public class LogoutVM : ProcessingVM
{
    private readonly AuthService _authService;
    private readonly NavigationManager _nav;
    public readonly CustomAuthenticationStateProvider _authStateProvider;

    public LogoutVM(AuthService authService, NavigationManager nav,
                    CustomAuthenticationStateProvider authStateProvider)
    {
        _authService = authService;
        _nav = nav;
        _authStateProvider = authStateProvider;
    }

    public async Task LogoutAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            var result = await _authService.Logout();

            if (result)
            {
                _authStateProvider.MarkUserAsLoggedOut();
                _nav.NavigateTo("/");
            }
        });
    }
}