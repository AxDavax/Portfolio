using ECommerce.ClientPortal.Providers;
using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.Auth.Login;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.ViewModels.Auth;

public class LoginVM : AuthVMBase
{
    public LoginRequest Request { get; set; } = new();

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public LoginVM(AuthService authService, NavigationManager nav, 
                   CustomAuthenticationStateProvider authStateProvider)
                   :base(authService, nav, authStateProvider) { }

    public async Task LoginAsync()
    {
        bool shouldRedirect = false;

        await RunCommandAsync(() => IsProcessing, async () =>
        {
            ErrorMessage = string.Empty;

            var result = await _authService.Login(Request);

            if (result == null)
            {
                ErrorMessage = "Invalid credentials";
                return;
            }
            else 
            
            _authStateProvider.MarkUserAsAuthenticated(result.Token);
            shouldRedirect = true;
        });

        if (shouldRedirect) 
            _nav.NavigateTo("/");
    }
}