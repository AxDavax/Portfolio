using ECommerce.Application.Interfaces;
using ECommerce.Contracts.Auth.Refresh;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.UseCases.Auth.Refresh;

public class RefreshHandler
{
    private readonly IRefreshTokenService _refreshTokens;
    private readonly IUserRepository _users;
    private readonly IUserAuthRepository _authUsers;
    private readonly IJwtService _jwt;

    public RefreshHandler(
        IRefreshTokenService refreshTokens,
        IUserRepository users,
        IUserAuthRepository authUsers,
        IJwtService jwt)
    {
        _refreshTokens = refreshTokens;
        _users = users;
        _authUsers = authUsers;
        _jwt = jwt;
    }

    public async Task<RefreshResponse> Handle(RefreshRequest request)
    {
        // 1. Validating the refresh token
        var storedToken = await _refreshTokens.GetRefreshTokenAsync(request.RefreshToken);
        if (storedToken == null)
            throw new UnauthorizedAccessException("Invalid token");

        if (storedToken.IsExpired)
            throw new UnauthorizedAccessException("expired refresh token");

        // 2. Loads user
        var user = await _users.GetByIdAsync(storedToken.UserId);
        if (user == null)
            throw new UnauthorizedAccessException("User not found");

        // 3. Loads sqlUser
        var sqlUser = await _authUsers.GetSqlUserByEmailAsync(user.Email);
        if (sqlUser == null)
            throw new UnauthorizedAccessException("User not found");

        // 4. Loads roles
        var roles = await _users.GetRolesAsync(user.Id);

        // 5. Generates a new JWT
        var newToken = _jwt.GenerateToken(user, roles);

        // 6. Generates a new refresh token
        var newRefreshToken = await _refreshTokens.RotateRefreshTokenAsync(storedToken);

        // 7. Returns the response
        return new RefreshResponse
        {
            Token = newToken,
            RefreshToken = newRefreshToken,
            UserId = user.Id,
            Email = user.Email,
            Roles = roles
        };
    }
}