using ECommerce.Domain.Auth.Interfaces;
using ECommerce.Domain.Auth.Models;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Infrastructure.Repositories.Auth;

public class ResetPasswordTokenRepository : IResetPasswordTokenRepository
{
    private readonly ISqlDataAccess _db;

    public ResetPasswordTokenRepository(ISqlDataAccess db)
    {
        _db = db;
    }

    public async Task CreateAsync(ResetPasswordToken token)
    {
        const string sql = """

            INSERT INTO 
                ResetPasswordTokens (Id, UserId, Token, CreatedAt, ExpiresAt)
            VALUES
                (@Id, @UserId, @Token, @CreatedAt, @ExpiresAt)

        """;

        await _db.ExecuteAsync(sql, token);
    }

    public async Task DeleteAsync(ResetPasswordToken token)
    {
        const string sql = """

            DELETE
            FROM
                ResetPasswordTokens
            WHERE
                Token = @Token
            
        """;

        if(token == null)
            throw new ArgumentNullException(nameof(token));

        await _db.ExecuteAsync(sql, new { Token = token.Token });
    }

    public async Task<ResetPasswordToken?> GetByTokenAsync(string token)
    {
        const string sql = """

            SELECT
                Id,
                UserId,
                Token,
                CreatedAt,
                ExpiresAt
            FROM
                ResetPasswordTokens
            WHERE
                Token = @Token

        """;

        return await _db.QuerySingleOrDefaultAsync<ResetPasswordToken, object>(
            sql, new { Token = token });
    }
}