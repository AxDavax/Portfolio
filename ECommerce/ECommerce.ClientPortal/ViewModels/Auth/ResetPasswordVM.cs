using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.Auth.ResetPassword;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.ClientPortal.ViewModels.Auth;

public class ResetPasswordVM : ProcessingVM
{
    private readonly AuthService _authService;

    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmation is required")]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords doesn't match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public ResetPasswordVM(AuthService authService)
    {
        _authService = authService;
    }

    public async Task SubmitAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            Message = string.Empty;

            var success = await _authService.ResetPassword(
                new ResetPasswordRequest
                {
                    Token = Token,
                    NewPassword = NewPassword,
                    ConfirmPassword = ConfirmPassword
                });

            Message = success
                ? "Your password has been successfully reset"
                : "Password cannot be reset";
        });
    }
}