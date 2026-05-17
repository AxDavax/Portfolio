using ECommerce.ClientPortal.Services.API.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.Services.API.Implementations;

public class RedirectApi : BaseApi, IRedirectApi
{
    public RedirectApi(HttpClient http, NavigationManager nav) : base(http, nav) { }

    public Task<string?> GetOAuthRedirectUrlAsync(string provider)
        => SafeGetRaw($"api/oauth/external/{provider}");
}