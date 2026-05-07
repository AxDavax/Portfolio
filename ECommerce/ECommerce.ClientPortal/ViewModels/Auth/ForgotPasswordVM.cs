using ECommerce.ClientPortal.Services.Auth;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.Auth.ForgotPassword;

namespace ECommerce.ClientPortal.ViewModels.Auth;

public class ForgotPasswordVM : ProcessingVM
{
    private readonly AuthService _authService;

    private string _email = string.Empty;
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    private string _message = string.Empty;
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public void OnInitialised()
    {
        Email = string.Empty;
        Message = string.Empty;
    }

    public ForgotPasswordVM(AuthService authService)
    {
        _authService = authService;
    }

    public async Task SubmitAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            Message = string.Empty;

            try
            {
                var success = await _authService.ForgotPassword(
                    new ForgotPasswordRequest { Email = Email });

                Message = success
                    ? "If this email exists, a reset link has been sent."
                    : "An error occurred. Please try again.";
            }
            catch (Exception ex)
            {
                Message = $"Unexpected error: {ex.Message}";
            }
        });
    }
}