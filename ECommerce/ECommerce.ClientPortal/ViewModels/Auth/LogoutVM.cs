using ECommerce.ClientPortal.Providers;
using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.ViewModels.Core;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.ViewModels.Auth;

public class LogoutVM : AuthVMBase
{
    public LogoutVM(AuthService authService, NavigationManager nav,
                    CustomAuthenticationStateProvider authStateProvider)
                    : base(authService, nav, authStateProvider) { }

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