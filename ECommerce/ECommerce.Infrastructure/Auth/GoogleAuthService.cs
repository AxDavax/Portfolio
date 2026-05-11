using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ProviderSettings = ECommerce.Application.Models.ProviderSettings;

namespace ECommerce.Infrastructure.Auth;

public class GoogleAuthService : IExternalAuthService
{
    private readonly ProviderSettings _settings;
    private readonly HttpClient _http;

    public GoogleAuthService(IOptionsSnapshot<ProviderSettings> settings, HttpClient http)
    {
        _settings = settings.Get("Google");
        _http = http;
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

        var tokenJson = await tokenResponse.Content.ReadFromJsonAsync<GoogleTokenResponse>();

        // 2. Get user info
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", tokenJson.AccessToken);

        var userInfo = await _http.GetFromJsonAsync<GoogleUserInfo>(
            "https://www.googleapis.com/oauth2/v3/userinfo");

        return new ExternalUserInfo
        {
            Email = userInfo.Email,
            Provider = "Google",
            ProviderId = userInfo.Sub
        };
    }
}