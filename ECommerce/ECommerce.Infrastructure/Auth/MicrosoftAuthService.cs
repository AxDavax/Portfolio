using ECommerce.Application.Models;
using ECommerce.Application.OAuth.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;


namespace ECommerce.Infrastructure.Auth;

public class MicrosoftAuthService : IExternalAuthService
{
    private readonly ProviderSettings _settings;
    private readonly HttpClient _http;

    public MicrosoftAuthService(
        IOptionsSnapshot<ProviderSettings> settings,
        IHttpClientFactory httpFactory)
    {
        _settings = settings.Get("Microsoft");
        _http = httpFactory.CreateClient();
    }

    public string GetAuthorizationUrl(string state)
    {
        return QueryHelpers.AddQueryString(
            "https://login.microsoftonline.com/common/oauth2/v2.0/authorize",
            new Dictionary<string, string>
            {
                ["client_id"] = _settings.ClientId,
                ["redirect_uri"] = _settings.RedirectUri,
                ["response_type"] = "code",
                ["scope"] = "openid email profile User.Read",
                ["state"] = state
            });
    }

    public async Task<ExternalUserInfo?> GetUserInfoAsync(string code)
    {
        // 1. Exchange code for access_token
        var tokenResponse = await _http.PostAsync(
            "https://login.microsoftonline.com/common/oauth2/v2.0/token",
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

        // 2. Retrieve user info from Microsoft Graph
        _http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenJson.AccessToken);

        var userInfo = await _http.GetFromJsonAsync<MicrosoftUserInfo>(
            "https://graph.microsoft.com/v1.0/me");

        if (userInfo == null)
            return null;

        // Microsoft sometimes returns email in Mail, sometimes in UserPrincipalName
        var email = !string.IsNullOrWhiteSpace(userInfo.Mail)
            ? userInfo.Mail
            : userInfo.UserPrincipalName;

        return new ExternalUserInfo
        {
            Email = email,
            Provider = "Microsoft",
            ProviderId = userInfo.Id
        };
    }
}