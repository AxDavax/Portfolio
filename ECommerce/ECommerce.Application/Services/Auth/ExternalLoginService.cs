using ECommerce.Application.Interfaces;
using ECommerce.Application.Interfaces.Auth;
using ECommerce.Application.Records;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Services.Auth;

public class ExternalLoginService : IExternalLoginService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserLoginRepository _userLoginRepository;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenService _tokenService;

    public ExternalLoginService(
        IUserRepository userRepository,
        IUserLoginRepository userLoginRepository,
        IJwtService jwtService,
        IRefreshTokenService tokenService)
    {
        _userRepository = userRepository;
        _userLoginRepository = userLoginRepository;
        _jwtService = jwtService;
        _tokenService = tokenService;
    }

    public async Task<ExternalLoginResult> HandleExternalUserAsync(
        string provider,
        string email,
        string providerUserId)
    {
        // 1. Check if login already exists
        var existingLogin = await _userLoginRepository.GetByProviderAsync(provider, providerUserId);

        User user;

        if (existingLogin != null)
        {
            // 2. Loads the associated user
            user = await _userRepository.GetByIdAsync(existingLogin.UserId);
        }
        else 
        {
            // 3. Check if the user exists by email
            user = await _userRepository.GetByEmailAsync(email);

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
                await _userRepository.CreateAsync(user); 
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

            await _userLoginRepository.AddAsync(login);
        }

        // 6. Loads the roles
        var roles = await _userRepository.GetRolesAsync(user.Id);

        // 7. Generates the JWT
        var jwt = _jwtService.GenerateToken(user, roles);

        // 8. Generates the Refresh Token
        var refresh = await _tokenService.GenerateRefreshTokenAsync(user.Id);

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