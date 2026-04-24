using ECommerce.ClientPortal.Providers;
using ECommerce.ClientPortal.Services.Auth;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.ViewModels.Core;

public abstract class AuthVMBase : ProcessingVM
{
    protected readonly AuthService _authService;
    protected readonly NavigationManager _nav;
    protected readonly CustomAuthenticationStateProvider _authStateProvider;

    protected AuthVMBase(
        AuthService authService,
        NavigationManager nav,
        CustomAuthenticationStateProvider authStateProvider)
    {
        _authService = authService;
        _nav = nav;
        _authStateProvider = authStateProvider;
    }
}