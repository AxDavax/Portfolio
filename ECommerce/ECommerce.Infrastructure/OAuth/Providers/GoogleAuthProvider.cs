using ECommerce.Application.OAuth.Interfaces;
using ECommerce.Application.OAuth.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ProviderSettings = ECommerce.Application.OAuth.Models.ProviderSettings;

namespace ECommerce.Infrastructure.OAuth.Providers;

public class GoogleAuthProvider : IExternalAuthProvider
{
    private readonly ProviderSettings _settings;
    private readonly HttpClient _http;

    public GoogleAuthProvider(IOptionsSnapshot<ProviderSettings> settings, 
        IHttpClientFactory httpFactory)
    {
        _settings = settings.Get("Google");
        _http = httpFactory.CreateClient();
    }

    public string GetAuthorizationUrl(string state)
    {
        var url = QueryHelpers.AddQueryString(
            "https://accounts.google.com/o/oauth2/v2/auth",
            new Dictionary<string, string>
            {
                ["client_id"] = _settings.ClientId,
                ["redirect_uri"] = _settings.RedirectUri,
                ["response_type"] = "code",
                ["scope"] = "openid email profile",
                ["state"] = state,
                ["access_type"] = "offline",
                ["prompt"] = "consent"
            });

        return url;
    }

    public async Task<ExternalUserInfo?> GetUserInfoAsync(string code)
    {
        // 1. Exchange code for tokens
        var tokenResponse = await _http.PostAsync(
            "https://oauth2.googleapis.com/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = _settings.ClientId,
                ["client_secret"] = _settings.ClientSecret,
                ["code"] = code,
                ["grant_type"] = "authorization_code",
                ["redirect_uri"] = _settings.RedirectUri
            }));

        if (!tokenResponse.IsSuccessStatusCode)
            return null;

        var tokenJson = await tokenResponse.Content.ReadFromJsonAsync<ProviderTokenResponse>();
        if (tokenJson == null || string.IsNullOrWhiteSpace(tokenJson.AccessToken))
            return null;

        // 2. Get user info
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", tokenJson.AccessToken);

        var userInfo = await _http.GetFromJsonAsync<GoogleUserInfo>(
            "https://www.googleapis.com/oauth2/v3/userinfo");

        if (userInfo == null)
            return null;

        return new ExternalUserInfo
        {
            Email = userInfo.Email,
            Provider = "Google",
            ProviderId = userInfo.Sub
        };
    }
}