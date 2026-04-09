using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.ViewModels.Auth;

public class UserMenuVM 
{
    private readonly NavigationManager _nav;

    public UserMenuVM(NavigationManager nav)
    {
        _nav = nav;
    }

    public void GoToLogin() => _nav.NavigateTo("/login");
    public void GoToRegister() => _nav.NavigateTo("/register");
    public void GoToProfile() => _nav.NavigateTo("/profile");
    public void Logout() => _nav.NavigateTo("/logout");
}