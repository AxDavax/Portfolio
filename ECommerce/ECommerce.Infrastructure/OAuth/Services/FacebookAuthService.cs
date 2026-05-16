using ECommerce.Application.OAuth.Interfaces;
using ECommerce.Application.OAuth.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace ECommerce.Infrastructure.OAuth.Services;

public class FacebookAuthService : IExternalAuthProvider
{
    private readonly ProviderSettings _settings;
    private readonly HttpClient _http;

    public FacebookAuthService(
            IOptionsSnapshot<ProviderSettings> settings,
            IHttpClientFactory httpFactory)
    {
        _settings = settings.Get("Facebook");
        _http = httpFactory.CreateClient();
    }

    public string GetAuthorizationUrl(string state)
    {
        return QueryHelpers.AddQueryString(
            "https://www.facebook.com/v18.0/dialog/oauth",
            new Dictionary<string, string>
            {
                ["client_id"] = _settings.ClientId,
                ["redirect_uri"] = _settings.RedirectUri,
                ["response_type"] = "code",
                ["state"] = state,
                ["scope"] = "email"
            });
    }

    public async Task<ExternalUserInfo?> GetUserInfoAsync(string code)
    {
        // 1. Exchange code for access_token
        var tokenResponse = await _http.GetAsync(
            QueryHelpers.AddQueryString(
                "https://graph.facebook.com/v18.0/oauth/access_token",
                new Dictionary<string, string>
                {
                    ["client_id"] = _settings.ClientId,
                    ["client_secret"] = _settings.ClientSecret,
                    ["redirect_uri"] = _settings.RedirectUri,
                    ["code"] = code
                }));

        if (!tokenResponse.IsSuccessStatusCode)
            return null;

        var tokenJson = await tokenResponse.Content.ReadFromJsonAsync<ProviderTokenResponse>();
        if (tokenJson == null || string.IsNullOrWhiteSpace(tokenJson.AccessToken))
            return null;

        // 2. Get user info
        var userInfo = await _http.GetFromJsonAsync<FacebookUserInfo>(
            QueryHelpers.AddQueryString(
                "https://graph.facebook.com/me",
                new Dictionary<string, string>
                {
                    ["fields"] = "id,email",
                    ["access_token"] = tokenJson.AccessToken
                }));

        if (userInfo == null)
            return null;

        return new ExternalUserInfo
        {
            Email = userInfo.Email,
            Provider = "Facebook",
            ProviderId = userInfo.Id
        };
    }
}