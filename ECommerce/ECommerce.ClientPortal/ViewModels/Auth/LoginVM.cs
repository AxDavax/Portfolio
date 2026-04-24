using ECommerce.ClientPortal.Providers;
using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.Auth.Login;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.ViewModels.Auth;

public class LoginVM : AuthVMBase
{
    public LoginRequest Request { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public LoginVM(AuthService authService, NavigationManager nav, 
                   CustomAuthenticationStateProvider authStateProvider)
                   :base(authService, nav, authStateProvider) { }

    public async Task LoginAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            ErrorMessage = string.Empty;
            var result = await _authService.Login(Request);

            if (result != null)
            {
                _authStateProvider.MarkUserAsAuthenticated(result.Token);
            }
            else 
            {
                ErrorMessage = "Invalid credentials";
                return;
            }

            _nav.NavigateTo("/");
        });
    }
}