using ECommerce.ClientPortal.Providers;
using ECommerce.ClientPortal.Services.API;
using ECommerce.Contracts.Auth;
using ECommerce.Contracts.Auth.ForgotPassword;
using ECommerce.Contracts.Auth.Login;
using ECommerce.Contracts.Auth.Logout;
using ECommerce.Contracts.Auth.Me;
using ECommerce.Contracts.Auth.Refresh;
using ECommerce.Contracts.Auth.Register;
using ECommerce.Contracts.Auth.ResetPassword;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;

namespace ECommerce.ClientPortal.Services.Auth;

public class AuthService : BaseApi
{
    private readonly TokenStorageService _tokenStorage;

    public AuthService(
        HttpClient http,
        NavigationManager nav,
        TokenStorageService tokenStorage) : base(http, nav)
    {
        _tokenStorage = tokenStorage;
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
    }

    public Task<string?> GetTokenAsync() => _tokenStorage.GetTokenAsync();

    private async Task<AuthResponse?> ApplyAuthenticationAsync(AuthResponse result)
    {
        if (result == null || string.IsNullOrWhiteSpace(result.Token))
            return null;

        await _tokenStorage.SaveAsync(result.Token, result.RefreshToken, result.Expiration);

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", result.Token);

        return result;
    }


    public async Task<AuthResponse?> Login(LoginRequest request)
    {
        var result = await SafePost<LoginResponse>("api/auth/login", request);
        return await ApplyAuthenticationAsync(result!);
    }

    public async Task<AuthResponse?> Register(RegisterRequest request) 
    {
        var result = await SafePost<RegisterResponse>("api/auth/register", request);
        return await ApplyAuthenticationAsync(result!);
    }

    public async Task<AuthResponse?> Refresh()
    {
        var refreshToken = await _tokenStorage.GetRefreshTokenAsync();

        if (string.IsNullOrWhiteSpace(refreshToken)) return null;

        var request = new RefreshRequest
        {
            RefreshToken = refreshToken
        };

        var result = await SafePost<RefreshResponse>("api/auth/refresh", request);

        if (result == null || result.Success == false)
        {
            await _tokenStorage.ClearAsync();
            return null;
        }

        return await ApplyAuthenticationAsync(result);
    }

    public async Task<bool> Logout()
    {
        var refreshToken = await _tokenStorage.GetRefreshTokenAsync();

        var result = await SafePost<bool>("api/auth/logout", new LogoutRequest
        {
            RefreshToken = refreshToken!
        });

        if (result) 
        { 
            await _tokenStorage.ClearAsync();
            _http.DefaultRequestHeaders.Authorization = null;
        }

        return result;
    }

    public Task<bool> ForgotPassword(ForgotPasswordRequest request)
        => SafePost<bool>("api/auth/forgot-password", request);

    public Task<bool> ResetPassword(ResetPasswordRequest request)
        => SafePost<bool>("api/auth/reset-password", request);

    public async Task<MeResponse> Me() => await SafeGet<MeResponse>("api/auth/me");
}