using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace ECommerce.Infrastructure.Auth;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly ISqlDataAccess _db;

    private readonly IConfiguration _config;

    public RefreshTokenService(IConfiguration config, ISqlDataAccess db)
    {
        _db = db;
        _config = config;
    }

    public async Task<string> GenerateRefreshTokenAsync(Guid userId)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        const string sql = """

            INSERT INTO 
                RefreshTokens (UserId, Token, ExpiryDate)
            VALUES
                (@UserId, @Token, DATEADD(day, 7, GETUTCDATE()))

        """;

        await _db.ExecuteAsync(sql, new { UserId = userId, Token = token });
        return token;
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        const string sql = """

            SELECT
                UserId, Token, ExpiryDate
            FROM
                RefreshTokens
            WHERE
                Token = @Token

        """;

        return await _db.QuerySingleOrDefaultAsync<RefreshToken, dynamic>(
            sql, new { Token = token });
    }

    public async Task ReplaceRefreshTokenAsync(Guid userId, string oldToken, string newToken)
    {
        const string sql = """
        
            DELETE
            FROM
                RefreshTokens
            WHERE
                UserId = @UserId AND Token = @OldToken;

            INSERT 
            INTO 
                RefreshTokens (UserId, Token, ExpiryDate)
            VALUES 
                (@UserId, @NewToken, DATEADD(day, 7, GETUTCDATE()));

        """;

        await _db.ExecuteAsync(sql, new 
        { 
            UserId = userId, 
            OldToken = oldToken, 
            NewToken = newToken 
        });
    }

    public async Task<string> RotateRefreshTokenAsync(RefreshToken oldToken)
    {
        var newToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        const string sql = """

            DELETE 
            FROM 
                RefreshTokens
            WHERE 
                Token = @OldToken;

            INSERT INTO 
                RefreshTokens (UserId, Token, ExpiryDate)
            VALUES 
                (@UserId, @NewToken, DATEADD(day, 7, GETUTCDATE()));

        """;

        await _db.ExecuteAsync(sql, new
        {
            UserId = oldToken.UserId,
            OldToken = oldToken.Token,
            NewToken = newToken
        });

        return newToken;
    }

    public async Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        const string sql = """

            SELECT 
                Token 
            FROM 
                RefreshTokens
            WHERE 
                UserId = @UserId
                AND Token = @Token
                AND ExpiryDate > GETUTCDATE()

        """;

        var result = await _db.QuerySingleOrDefaultAsync<string, dynamic>(
            sql, new { UserId = userId, Token = refreshToken });

        return result != null;
    }

    public async Task DeleteRefreshTokenAsync(string refreshToken)
    {
        const string sql = """

            DELETE
            FROM
                RefreshTokens
            WHERE
                Token = @Token

        """;

        await _db.ExecuteAsync(sql, new { Token = refreshToken });
    }
}