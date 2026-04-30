using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ECommerce.ClientPortal.ViewModels.Core;

public class AuthUserVM : BaseVM
{
    private readonly AuthenticationStateProvider _authStateProvider;

    private readonly TaskCompletionSource<bool> _ready = new();

    public Task WaitUntilReadyAsync() => _ready.Task;

    private ClaimsPrincipal _user = new(new ClaimsIdentity());
    public ClaimsPrincipal User 
    { 
        get => _user;
        private set => SetProperty(ref _user, value); 
    }

    public bool IsReady { get; private set; }

    public AuthUserVM(AuthenticationStateProvider authenticationStateProvider) 
    {
        _authStateProvider = authenticationStateProvider;

        _authStateProvider.AuthenticationStateChanged += OnAuthStateChanged;
    }

    private async void OnAuthStateChanged(Task<AuthenticationState> task)
    {
        var authState = await task;
        User = authState.User;

        IsReady = true;
        _ready.TrySetResult(true);

        OnPropertyChanged(nameof(IsReady));
    }

    public async Task LoadAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        User = authState.User;

        IsReady = true;
        _ready.TrySetResult(true);

        OnPropertyChanged(nameof(IsReady));
    }
}