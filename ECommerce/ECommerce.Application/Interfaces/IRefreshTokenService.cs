using ECommerce.Application.Models;

namespace ECommerce.Application.Interfaces;

public interface IRefreshTokenService
{
    Task<string> GenerateRefreshTokenAsync(Guid userId);
    Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken);
    Task ReplaceRefreshTokenAsync(Guid userId, string oldToken, string newToken);
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
    Task<string> RotateRefreshTokenAsync(RefreshToken oldToken);
    Task DeleteRefreshTokenAsync(string refreshToken);
}