using ECommerce.ClientPortal.Providers;
using ECommerce.ClientPortal.Services.API.Implementations;
using ECommerce.Contracts.Auth.ForgotPassword;
using ECommerce.Contracts.Auth.Login;
using ECommerce.Contracts.Auth.Logout;
using ECommerce.Contracts.Auth.Register;
using ECommerce.Contracts.Auth.ResetPassword;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;

namespace ECommerce.ClientPortal.Services.Auth;

public class AuthService : BaseApi
{
    private readonly TokenStorageService _tokenStorage;
    private readonly CustomAuthenticationStateProvider _authStateProvider;

    public AuthService(
        HttpClient http,
        NavigationManager nav,
        TokenStorageService tokenStorage,
        CustomAuthenticationStateProvider authStateProvider) : base(http, nav)
    {
        _tokenStorage = tokenStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task InitializeAsync()
    {
        var token = await _tokenStorage.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token)) return;

        var isExpired = await _tokenStorage.IsExpiredAsync();
        if (isExpired)
        {
            await Logout();
            return;
        }

        _http.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
        
        await _authStateProvider.MarkUserAsAuthenticated(token);
    }

    public async Task<bool> Login(LoginRequest request)
    {
        var result = await SafePost<LoginResponse>("api/auth/login", request);

        if (result == null) return false;

        await _tokenStorage.SaveAsync(result.Token, result.RefreshToken, result.Expiration);

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", result.Token);

        await _authStateProvider.MarkUserAsAuthenticated(result.Token);

        return true;
    }

    public async Task<bool> Register(RegisterRequest request) 
    {
        var result = await SafePost<RegisterResponse>("api/auth/register", request);
        
        if (result == null) return false;

        await _tokenStorage.SaveAsync(result.Token, result.RefreshToken, result.Expiration);

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", result.Token);

        await _authStateProvider.MarkUserAsAuthenticated(result.Token);

        return true;
    }

    public async Task Logout()
    {
        var refreshToken = await _tokenStorage.GetRefreshTokenAsync();

        var result = await SafePost<bool>("api/auth/logout", new LogoutRequest
        {
            RefreshToken = refreshToken!
        });

        if (result) 
        { 
            await _tokenStorage.ClearAsync();
            _authStateProvider.MarkUserAsLoggedOut();

            _http.DefaultRequestHeaders.Authorization = null;
        }
    }

    public Task<bool> ForgotPassword(ForgotPasswordRequest request)
        => SafePost<bool>("api/auth/forgot-password", request);

    public Task<bool> ResetPassword(ResetPasswordRequest request)
        => SafePost<bool>("api/auth/reset-password", request);
}