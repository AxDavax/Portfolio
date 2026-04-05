using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ECommerce.ClientPortal.Services.Auth;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);

        return Task.FromResult(new AuthenticationState(user));
    }
}