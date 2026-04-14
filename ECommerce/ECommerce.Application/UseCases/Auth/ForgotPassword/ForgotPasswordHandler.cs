using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Contracts.Auth.ForgotPassword;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.UseCases.Auth.ForgotPassword;

public class ForgotPasswordHandler
{
    private readonly IUserRepository _users;
    private readonly IResetPasswordTokenService _resetTokens;
    private readonly IEmailService _email;   

    public ForgotPasswordHandler(
        IUserRepository users,
        IResetPasswordTokenService resetTokens,
        IEmailService email)
    {
        _users = users;
        _resetTokens = resetTokens;
        _email = email;
    }

    public async Task<ForgotPasswordResponse> HandleAsync(ForgotPasswordRequest request)
    {
        // 1. Loads user (domain entity)
        var user = await _users.GetByEmailAsync(request.Email);

        // 2. Always return Success = true for security purposes
        if(user == null) 
            return new ForgotPasswordResponse { Success = true };

        // 3. Generates a unique token
        var token = Guid.NewGuid().ToString("N");

        // 4. Saves the token in database
        var resetToken = new ResetPasswordToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = token,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        // 5. Sends the email 
        var resetLink = $"https://localhost:7083/reset-password?token={token}";

        var html = $@"
            <h2>Password Reset</h2>
            <p>Click the link below to reset your password:</p>
            <p><a href=""{resetLink}"">Reset Password</a></p>
            <p>This link expires in 1 hour.</p>
        ";

        await _email.SendAsync(user.Email, "Reset your password", html);

        // 6. Always Success = true
        return new ForgotPasswordResponse { Success = true };
    }
}