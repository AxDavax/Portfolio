using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.Auth.ResetPassword;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.ClientPortal.ViewModels.Auth;

public class ResetPasswordVM : ProcessingVM
{
    private readonly AuthService _authService;

    private string _token = string.Empty;
    public string Token     
    {
        get => _token;
        set => SetProperty(ref _token, value);
    }

    private string _newPassword = string.Empty;
    [Required(ErrorMessage = "Password is required")]
    public string NewPassword
    {
        get => _newPassword;
        set => SetProperty(ref _newPassword, value);
    }

    private string _confirmPassword = string.Empty;
    [Required(ErrorMessage = "Confirmation is required")]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords doesn't match")]
    public string ConfirmPassword 
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }

    private string _message = string.Empty;
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public ResetPasswordVM(AuthService authService)
    {
        _authService = authService;
    }

    public void OnInitialised(string token)
    {
        Token = token;
        NewPassword = string.Empty;
        ConfirmPassword = string.Empty;
        Message = string.Empty;
    }

    public async Task SubmitAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            Message = string.Empty;

            try 
            { 
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
            }
            catch (Exception ex)
            {
                Message = $"Unexpected error: {ex.Message}";
            }
        });
    }
}