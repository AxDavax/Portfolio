using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Domain.Interfaces;
using System.Security.Cryptography;

namespace ECommerce.Infrastructure.Auth;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly ISqlDataAccess _db;

    public RefreshTokenService(ISqlDataAccess db)
    {
        _db = db;
    }

    public async Task<string> GenerateRefreshTokenAsync(Guid userId)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        const string sql = """

            INSERT INTO 
                RefreshTokens (UserId, Token)
            VALUES
                (@UserId, @Token)

        """;

        await _db.ExecuteAsync(sql, new { UserId = userId, Token = token });
        return token;
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        const string sql = """

            SELECT
                UserId, Token
            FROM
                RefreshTokens
            WHERE
                Token = @Token

        """;

        return await _db.QuerySingleOrDefaultAsync<RefreshToken, object>(
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
                RefreshTokens (UserId, Token)
            VALUES 
                (@UserId, @NewToken);

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
                RefreshTokens (UserId, Token)
            VALUES 
                (@UserId, @NewToken);

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
                AND ExpiresAt > GETUTCDATE()

        """;

        var result = await _db.QuerySingleOrDefaultAsync<string, object>(
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