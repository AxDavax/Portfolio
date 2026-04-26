using ECommerce.ClientPortal.Services.Auth;
using ECommerce.Contracts.Auth.Refresh;
using System.Net.Http.Json;

namespace ECommerce.ClientPortal.Services.Http;

public class AuthHttpMessageHandler : DelegatingHandler
{
    private readonly HttpClient _refreshClient;
    private readonly TokenStorageService _tokenStorage;

    public AuthHttpMessageHandler(IConfiguration config, TokenStorageService tokenStorage)
    {
        _tokenStorage = tokenStorage;

        _refreshClient = new HttpClient
        {
            BaseAddress = new Uri(config["Api:BaseUrl"]!)
        };
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenStorage.GetTokenAsync();
        if(!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            var refreshed = await TryRefreshTokenAsync(cancellationToken);

            if (refreshed?.Success == true)
            {
                var newToken = await _tokenStorage.GetTokenAsync(); 
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newToken);

                return await base.SendAsync(request, cancellationToken); 
            }
        }

        return response;
    }

    private async Task<RefreshResponse?> TryRefreshTokenAsync(CancellationToken cancellationToken)
    {
        var refreshToken = await _tokenStorage.GetRefreshTokenAsync();
        if (string.IsNullOrWhiteSpace(refreshToken))
            return null;

        var request = new RefreshRequest { RefreshToken = refreshToken };

        var httpResponse = await _refreshClient.PostAsJsonAsync(
            "api/auth/refresh",
            request,
            cancellationToken
        );

        if (!httpResponse.IsSuccessStatusCode) return null;

        var result = await httpResponse.Content.ReadFromJsonAsync<RefreshResponse>(cancellationToken: cancellationToken);

        if (result == null || !result.Success) return null;

        await _tokenStorage.SaveAsync(result.Token, result.RefreshToken, result.Expiration);

        return result;
    }
}