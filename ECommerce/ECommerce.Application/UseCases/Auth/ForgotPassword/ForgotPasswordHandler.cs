using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Contracts.Auth.ForgotPassword;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.UseCases.Auth.ForgotPassword;

public class ForgotPasswordHandler
{
    private readonly IUserRepository _users;
    private readonly IEmailService _email;
    private readonly IEmailTemplateService _templates;

    public ForgotPasswordHandler(
        IUserRepository users,
        IEmailService email,
        IEmailTemplateService templates)
    {
        _users = users;
        _email = email;
        _templates = templates;
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

        var html = await _templates.RenderAsync("ResetPassword",
            new ResetPasswordModel { Link = resetLink }
        );

        await _email.SendAsync(user.Email, "Reset your password", html);

        // 6. Always Success = true
        return new ForgotPasswordResponse { Success = true };
    }
}