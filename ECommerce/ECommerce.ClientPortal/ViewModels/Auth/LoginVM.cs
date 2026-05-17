using ECommerce.ClientPortal.Providers;
using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.Auth.Login;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.ViewModels.Auth;

public class LoginVM : AuthVMBase
{
    private readonly NavigationManager _nav;

    public LoginRequest Request { get; set; } = new();

    public LoginVM(AuthService authService, NavigationManager nav, 
                   CustomAuthenticationStateProvider authStateProvider)
                   :base(authService, nav, authStateProvider) 
    {
        _nav = nav;
    }

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

    public void OnInitialized()
    {
        var uri = _nav.ToAbsoluteUri(_nav.Uri);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

        var error = query["error"];

        if (string.IsNullOrEmpty(error))
            return;

        ErrorMessage = error switch
        {
            "invalid_oauth_response" => "The OAuth provider returned an invalid response.",
            "oauth_callback_failed" => "OAuth login failed. Please try again.",
            "oauth_redirect_failed" => "Unable to contact the OAuth provider.",
            _ => "An unknown authentication error occurred."
        };
    }
}