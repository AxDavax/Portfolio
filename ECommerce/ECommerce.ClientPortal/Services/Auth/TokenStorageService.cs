using ECommerce.ClientPortal.Services.Storage;

namespace ECommerce.ClientPortal.Services.Auth;

public class TokenStorageService
{
    private readonly LocalStorageService _localStorage;

    private const string TokenKey = "authToken";
    private const string RefreshTokenKey = "refreshToken";
    private const string ExpirationKey = "tokenExpiration";

    public TokenStorageService(LocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task SaveAsync(string token, string refreshToken, DateTime expiration)
    {
        await _localStorage.SetItemAsync(TokenKey, token);
        await _localStorage.SetItemAsync(RefreshTokenKey, refreshToken);
        await _localStorage.SetItemAsync(ExpirationKey, expiration);
    }

    public Task<string?> GetTokenAsync() => _localStorage.GetItemAsync<string>(TokenKey);

    public Task<string?> GetRefreshTokenAsync()
        => _localStorage.GetItemAsync<string>(RefreshTokenKey);

    public async Task<DateTime?> GetExpirationAsync()
        => await _localStorage.GetItemAsync<DateTime>(ExpirationKey);

    public async Task ClearAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
        await _localStorage.RemoveItemAsync(RefreshTokenKey);
        await _localStorage.RemoveItemAsync(ExpirationKey);
    }

    public async Task<bool> IsExpiredAsync()
    {
        var expiration = await GetExpirationAsync();
        if (expiration != null)
            return true;

        return expiration <= DateTime.UtcNow;
    }
}