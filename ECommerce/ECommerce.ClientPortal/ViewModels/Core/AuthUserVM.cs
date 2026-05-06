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

    private bool _isReady;
    public bool IsReady
    {
        get => _isReady;
        private set => SetProperty(ref _isReady, value);
    }

    /// <summary>
    /// Gets the user ID from the "uid" claim injected by the JWT.
    /// </summary>
    public string? UserId => User?.FindFirst("uid")?.Value;

    public AuthUserVM(AuthenticationStateProvider authenticationStateProvider) 
    {
        _authStateProvider = authenticationStateProvider;

        // AuthenticationStateChanged's signature expects a void delegate, not Task.
        _authStateProvider.AuthenticationStateChanged += HandleAuthStateChangedWrapper;
    }

    private void HandleAuthStateChangedWrapper(Task<AuthenticationState> task)
    {
        _ = HandleAuthStateChanged(task);
    }

    private ClaimsPrincipal MapClaims(ClaimsPrincipal original)
    {
        var identity = original.Identity as ClaimsIdentity;

        var newIdentity = new ClaimsIdentity(
            identity?.Claims,
            identity?.AuthenticationType,
            ClaimTypes.Name,
            ClaimTypes.Role
        );

        return new ClaimsPrincipal(newIdentity);
    }


    private async Task HandleAuthStateChanged(Task<AuthenticationState> task)
    {
        var authState = await task;
        User = MapClaims(authState.User);

        IsReady = true;
        _ready.TrySetResult(true);

        NotifyStateChanged();
    }

    public async Task LoadAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        User = MapClaims(authState.User);

        IsReady = true;
        _ready.TrySetResult(true);

        NotifyStateChanged();
    }

    public void Dispose()
    {
        _authStateProvider.AuthenticationStateChanged -= HandleAuthStateChangedWrapper;
    }
}