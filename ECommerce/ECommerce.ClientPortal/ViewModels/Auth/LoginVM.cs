using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.Auth.Login;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.ViewModels.Auth;

public class LoginVM : ProcessingVM
{
    public LoginRequest Model { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public readonly AuthService _authService;
    public readonly NavigationManager _nav;

    public LoginVM(AuthService authService, NavigationManager nav)
    {
        _authService = authService;
        _nav = nav;
    }

    public async Task LoginAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            ErrorMessage = string.Empty;
            var success = await _authService.Login(Model);

            if (!success)
            {
                ErrorMessage = "Invalid credentials";
                return;
            }

            _nav.NavigateTo("/");
        });
    }
}