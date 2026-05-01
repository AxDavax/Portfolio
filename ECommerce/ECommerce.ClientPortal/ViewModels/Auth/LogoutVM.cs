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
        bool shouldRedirect = false;

        await RunCommandAsync(() => IsProcessing, async () =>
        {
            try
            {
                var result = await _authService.Logout();

                if (result)
                {
                    _authStateProvider.MarkUserAsLoggedOut();
                    shouldRedirect = true;
                }
            }
            catch (Exception ex)
            {
                // expose a message
            }
        });

        if (shouldRedirect)
            _nav.NavigateTo("/");
    }
}