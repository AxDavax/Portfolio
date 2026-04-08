using ECommerce.Application.Interfaces;
using ECommerce.Contracts.Auth.Login;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.UseCases.Auth.Login;

public class LoginHandler
{
    private readonly IUserRepository _users;
    private readonly IUserAuthRepository _authUsers;
    private readonly IPasswordService _passwords;
    private readonly IJwtService _jwt;
    private readonly IRefreshTokenService _refreshToken;

    public LoginHandler(
        IUserRepository users,
        IUserAuthRepository authUsers,
        IPasswordService passwords,
        IJwtService jwt,
        IRefreshTokenService refreshToken)
    {
        _users = users;
        _authUsers = authUsers;
        _passwords = passwords;
        _jwt = jwt;
        _refreshToken = refreshToken;
    }

    public async Task<LoginResponse> Handle(LoginRequest request)
    {
        // 1. Loads user (domain entity)
        var user = await _users.GetByEmailAsync(request.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        // 2. Loads sqlUser (to hash/salt)
        var sqlUser = await _authUsers.GetSqlUserByEmailAsync(request.Email);
        if (sqlUser == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        // 3. Verify password
        var validPassword = _passwords.VerifyPassword(
            request.Password,
            sqlUser.PasswordHash,
            sqlUser.PasswordSalt
        );

        if (!validPassword)
            throw new UnauthorizedAccessException("Invalid credentials");

        // 4. Loading roles
        var roles = await _users.GetRolesAsync(user.Id);

        // 5. Generating JWT
        var token = _jwt.GenerateToken(user.Id, user.Email, roles);

        // 6. Generating RefreshToken
        var refreshToken = await _refreshToken.GenerateRefreshTokenAsync(user.Id);

        // 7. Returning response
        return new LoginResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            UserId = user.Id,
            Email = user.Email,
            Roles = roles
        };
    }
}