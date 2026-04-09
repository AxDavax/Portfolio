using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.Auth.Register;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.ViewModels.Auth;

public class RegisterVM : ProcessingVM
{
    public RegisterRequest Model { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    private readonly AuthService _authService;
    private readonly NavigationManager _nav;

    public RegisterVM(AuthService authService, NavigationManager nav)
    {
        _authService = authService;
        _nav = nav;
    }

    public async Task RegisterAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            ErrorMessage = string.Empty;
            var success = await _authService.Register(Model);

            if (!success)
            {
                ErrorMessage = "Impossible to create the account";
                return;
            }

            _nav.NavigateTo("/");
        });
    }
}