using ECommerce.ClientPortal.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECommerce.ClientPortal.Providers;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly TokenStorageService _tokenStorage;

    public CustomAuthenticationStateProvider(TokenStorageService tokenStorage)
    {
        _tokenStorage = tokenStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void MarkUserAsLoggedOut()
    {
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