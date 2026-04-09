using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.ViewModels.Core;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.ViewModels.Auth;

public class LogoutVM : ProcessingVM
{
    private readonly AuthService _authService;
    private readonly NavigationManager _nav;

    public LogoutVM(AuthService authService, NavigationManager nav)
    {
        _authService = authService;
        _nav = nav;
    }

    public async Task LogoutAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            await _authService.Logout();
            _nav.NavigateTo("/");
        });
    }
}