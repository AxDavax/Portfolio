using ECommerce.Application.Interfaces;
using ECommerce.Application.Interfaces.Auth;
using ECommerce.Application.Records;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Services.Auth;

public class ExternalLoginService : IExternalLoginService
{
    private readonly IUserRepository _userRepo;
    private readonly IUserLoginRepository _userLoginRepo;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenRepository _refreshTokenRepo;

    public ExternalLoginService(
        IUserRepository userRepo,
        IUserLoginRepository userLoginRepo,
        IJwtService jwtService,
        IRefreshTokenRepository refreshTokenRepo)
    {
        _userRepo = userRepo;
        _userLoginRepo = userLoginRepo;
        _jwtService = jwtService;
        _refreshTokenRepo = refreshTokenRepo;
    }

    public async Task<ExternalLoginResult> HandleExternalUserAsync(
        string provider,
        string email,
        string providerUserId)
    {
        // 1. Check if login already exists
        var existingLogin = await _userLoginRepo.GetByProviderAsync(provider, providerUserId);

        User user;

        if (existingLogin != null)
        {
            // 2. Loads the associated user
            user = await _userRepo.GetByIdAsync(existingLogin.UserId);
        }
        else 
        {
            // 3. Check if the user exists by email
            user = await _userRepo.GetByEmailAsync(email);

            if (user == null)
            {
                // 4. Create a new user if not exists
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    IsActive = true
                };

                // or CreateAuthAsync(sqlUser) if you have a specific method for auth users
                await _userRepo.CreateAsync(user); 
            }

            // 5. Link provider
            var login = new UserLogin
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Provider = provider,
                ProviderUserId = providerUserId,
                CreatedAt = DateTime.UtcNow
            };

            await _userLoginRepo.AddAsync(login);
        }

        if(user == null || !user.IsActive)
        {
            return new ExternalLoginResult(
                Success: false,
                Email: email,
                ProviderUserId: providerUserId,
                JwtToken: null,
                RefreshToken: null,
                ErrorMessage: "User account is inactive or does not exist."
            );
        }

        // 6. Loads the roles
        var roles = await _userRepo.GetRolesAsync(user.Id);

        // 7. Generates the JWT
        var jwt = _jwtService.GenerateToken(user, roles);

        // 8. Generates the Refresh Token
        var refresh = await _refreshTokenRepo.GenerateRefreshTokenAsync(user.Id);

        // 9. Returns the response
        return new ExternalLoginResult(
            Success: true,
            Email: user.Email,
            ProviderUserId: providerUserId,
            JwtToken: jwt.Token,
            RefreshToken: refresh,
            ErrorMessage: null
        );
    }
}