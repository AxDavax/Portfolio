using ECommerce.ClientPortal.Providers;
using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.Auth.Register;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.ViewModels.Auth;

public class RegisterVM : AuthVMBase
{
    public RegisterRequest Request { get; set; } = new();

    public RegisterVM(AuthService authService, NavigationManager nav,
                      CustomAuthenticationStateProvider authStateProvider)
                        : base(authService, nav, authStateProvider) { }

    public async Task RegisterAsync()
    {
        bool shouldRedirect = false;

        await RunCommandAsync(() => IsProcessing, async () =>
        {
            ErrorMessage = string.Empty;
            var result = await _authService.Register(Request);

            if(result == null)
            {
                ErrorMessage = "Unable to create the account";
                return;
            }

            _authStateProvider.MarkUserAsAuthenticated(result.Token);
            shouldRedirect = true;
        });

        if (shouldRedirect) 
            _nav.NavigateTo("/");
    }
}