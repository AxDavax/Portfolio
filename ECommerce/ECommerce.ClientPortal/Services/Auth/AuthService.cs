using ECommerce.ClientPortal.Providers;
using ECommerce.Contracts.Auth.Login;
using ECommerce.Contracts.Auth.Logout;
using ECommerce.Contracts.Auth.Register;
using System.Net.Http.Json;

namespace ECommerce.ClientPortal.Services.Auth;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly TokenStorageService _tokenStorage;
    private readonly CustomAuthenticationStateProvider _authStateProvider;

    public AuthService(
        HttpClient http,
        TokenStorageService tokenStorage,
        CustomAuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _tokenStorage = tokenStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> Login(LoginRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", request);

        if(!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        await _tokenStorage.SaveAsync(result.Token, result.RefreshToken, result.Expiration);
        await _authStateProvider.MarkUserAsAuthenticated(result.Token);

        return true;
    }

    public async Task<bool> Register(RegisterRequest request) 
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", request);

        if(!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<RegisterResponse>();

        await _tokenStorage.SaveAsync(result.Token, result.RefreshToken, result.Expiration);
        await _authStateProvider.MarkUserAsAuthenticated(result.Token);

        return true;
    }

    public async Task Logout()
    {
        var refreshToken = await _tokenStorage.GetRefreshTokenAsync();

        await _http.PostAsJsonAsync("api/auth/logout", new LogoutRequest
        {
            RefreshToken = refreshToken!
        });

        await _tokenStorage.ClearAsync();
        _authStateProvider.MarkUserAsLoggedOut();
    }
}