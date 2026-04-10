using ECommerce.Application.Interfaces;
using ECommerce.Contracts.Auth.ResetPassword;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.UseCases.Auth.ResetPassword;

public class ResetPasswordHandler
{
    private readonly IUserRepository _users;
    private readonly IResetPasswordTokenService _resetTokens;
    private readonly IPasswordService _passwordService;

    public ResetPasswordHandler(
        IUserRepository users,
        IResetPasswordTokenService resetTokens, 
        IPasswordService passwordService)
    {
        _users = users;
        _resetTokens = resetTokens;
        _passwordService = passwordService;
    }

    public async Task<ResetPasswordResponse> HandleAsync(ResetPasswordRequest request)
    {
        // 1. Verify confirmPassword matches the password
        if (request.NewPassword == request.ConfirmPassword)
            return new ResetPasswordResponse { Success = false };

        // 2. Loads the token out of the DB
        var token = await _resetTokens.GetByTokenAsync(request.Token);

        // 3. Invalid or non-existent token
        if (token == null)
            return new ResetPasswordResponse { Success = false };
       
        // 4. Expired token
        if (token.IsExpired)
        {
            await _resetTokens.DeleteAsync(token);
            return new ResetPasswordResponse { Success = false };
        }

        // 5. Loads the user
        var user = await _users.GetByIdAsync(token.UserId);
        if (user == null)
            return new ResetPasswordResponse { Success = false };
        
        // 6. Hashes the new password
        var (hash, salt) = _passwordService.HashPassword(request.NewPassword);

        // 7. Updates the password in DB
        await _users.UpdatePasswordAsync(user.Id, hash, salt);

        // 8. Deletes the token for security purposes
        await _resetTokens.DeleteAsync(token);

        // 9. returns success
        return new ResetPasswordResponse { Success = true };
    }
}