using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Contracts.Auth.Register;
using ECommerce.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Application.UseCases.Auth.Register;

public class RegisterHandler
{
    private readonly IUserRepository _userRepo;
    private readonly IUserAuthRepository _sqlUserRepo;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private readonly IUserRoleService _userRoleService;
    private readonly IRoleService _roleService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IConfiguration _config;

    public RegisterHandler(
        IUserRepository userRepo,
        IUserAuthRepository sqlUserRepo,
        IPasswordService passwordService,
        IJwtService jwtService,
        IUserRoleService userRoleService,
        IRoleService roleService,
        IRefreshTokenService refreshTokenService,
        IConfiguration config)
    {
        _userRepo = userRepo;
        _sqlUserRepo = sqlUserRepo;
        _passwordService = passwordService;
        _jwtService = jwtService;
        _userRoleService = userRoleService;
        _roleService = roleService;
        _refreshTokenService = refreshTokenService;
        _config = config;
    }

    public async Task<RegisterResponse> HandleAsync(RegisterRequest request)
    {
        // 1. Verify if the user exists already
        var existing = await _userRepo.GetByEmailAsync(request.Email);
        if (existing != null)
            throw new InvalidOperationException("Email already registered");

        // 2. Hashes the password
        var (hash, salt) = _passwordService.HashPassword(request.Password);

        // 3. Creates the SqlUser object
        var user = new SqlUser
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = hash,
            PasswordSalt = salt,
            IsActive = true
        };

        // 4. Inserts the SqlUser in DB and gets his equivalent User object
        var createdUser = await _sqlUserRepo.CreateAuthAsync(user);

        // 5. Assign default role "Customer"
        var customerRoleId = await _roleService.GetIdByNameAsync("Customer");
        await _userRoleService.AssignRoleAsync(createdUser.Id, customerRoleId);

        // 6. Loads the roles (now includes "Customer")
        var roles = await _userRepo.GetRolesAsync(createdUser.Id);

        // 7. Generates the JWT
        var token = _jwtService.GenerateToken(createdUser, roles);

        // 8. Generates the Refresh Token
        var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(createdUser.Id);

        // 9. Calculates the expiration
        var expiration = DateTime.UtcNow.AddMinutes(
            int.Parse(_config["Jwt:ExpiresInMinutes"]!)
        );

        // 10. Returns the response
        return new RegisterResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            Expiration = expiration
        };
    }
}