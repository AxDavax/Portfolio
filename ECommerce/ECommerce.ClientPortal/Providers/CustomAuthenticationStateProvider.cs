using ECommerce.ClientPortal.Models;
using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.Services.State;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECommerce.ClientPortal.Providers;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly TokenStorageService _tokenStorage;
    private readonly UserSessionService _sessionService;

    public CustomAuthenticationStateProvider(TokenStorageService tokenStorage, 
        UserSessionService sessionService)
    {
        _tokenStorage = tokenStorage;
        _sessionService = sessionService;
    }

    private ClaimsPrincipal BuildUserFromToken(string token)
    {
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);

        var session = new UserSession
        {
            Email = user.FindFirst(ClaimTypes.Email)?.Value!,
            FirstName = user.FindFirst(ClaimTypes.GivenName)?.Value!,
            LastName = user.FindFirst(ClaimTypes.Surname)?.Value!,
            Roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList()
        };

        _sessionService.Set(session);

        return user;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            _sessionService.Clear();
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var user = BuildUserFromToken(token);
        return new AuthenticationState(user);
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        var user = BuildUserFromToken(token);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void MarkUserAsLoggedOut()
    {
        _sessionService.Clear();
        
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        var claims = new List<Claim>();

        foreach (var claim in token.Claims)
        {
            switch(claim.Type)
            {
                case JwtRegisteredClaimNames.GivenName:
                    claims.Add(new Claim(ClaimTypes.GivenName, claim.Value)); 
                    break;
                case JwtRegisteredClaimNames.FamilyName:
                    claims.Add(new Claim(ClaimTypes.Surname, claim.Value));
                    break;
                case JwtRegisteredClaimNames.Email:
                    claims.Add(new Claim(ClaimTypes.Email, claim.Value));
                    break;
                default:
                    claims.Add(claim);
                    break;
            }
        }

        return claims;
    }
}