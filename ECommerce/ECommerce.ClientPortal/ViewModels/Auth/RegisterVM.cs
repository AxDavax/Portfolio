using ECommerce.ClientPortal.Providers;
using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.Auth.Register;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.ViewModels.Auth;

public class RegisterVM : ProcessingVM
{
    public RegisterRequest Request { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    private readonly AuthService _authService;
    private readonly NavigationManager _nav;
    public readonly CustomAuthenticationStateProvider _authStateProvider;

    public RegisterVM(AuthService authService, NavigationManager nav,
                      CustomAuthenticationStateProvider authStateProvider)
    {
        _authService = authService;
        _nav = nav;
        _authStateProvider = authStateProvider;
    }

    public async Task RegisterAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            ErrorMessage = string.Empty;
            var result = await _authService.Register(Request);

            if (result != null)
            {
                _authStateProvider.MarkUserAsAuthenticated(result.Token);
            }
            else
            {
                ErrorMessage = "Unable to create the account";
                return;
            }

            _nav.NavigateTo("/");
        });
    }
}