using ECommerce.ClientPortal.Services.Auth;

namespace ECommerce.ClientPortal.Services.Http;

public class AuthHttpMessageHandler : DelegatingHandler
{
    private readonly AuthService _authService;
    private readonly TokenStorageService _tokenStorage;

    public AuthHttpMessageHandler(AuthService authService, TokenStorageService tokenStorage)
    {
        _authService = authService;
        _tokenStorage = tokenStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenStorage.GetTokenAsync();
        if(!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            var refreshed = await _authService.Refresh();

            if (refreshed != null && refreshed.Success)
            {
                token = await _tokenStorage.GetTokenAsync(); 
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", refreshed.Token);

                return await base.SendAsync(request, cancellationToken); 
            }
        }

        return response;
    }
}