using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ECommerce.ClientPortal.ViewModels.Core;

public class AuthUserVM : BaseVM
{
    private readonly AuthenticationStateProvider _authStateProvider;

    private ClaimsPrincipal _user;
    public ClaimsPrincipal User 
    { 
        get => _user;
        private set => SetProperty(ref _user, value); 
    }

    public bool IsReady => User != null;

    public AuthUserVM(AuthenticationStateProvider authenticationStateProvider) 
    {
        _authStateProvider = authenticationStateProvider;

        _authStateProvider.AuthenticationStateChanged += OnAuthStateChanged;
    }

    private async void OnAuthStateChanged(Task<AuthenticationState> task)
    {
        var authState = await task;
        User = authState.User;

        OnPropertyChanged(nameof(IsReady));
    }

    public async Task LoadAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        User = authState.User;

        OnPropertyChanged(nameof(IsReady));
    }
}