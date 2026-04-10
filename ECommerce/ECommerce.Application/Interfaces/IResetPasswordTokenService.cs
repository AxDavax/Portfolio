using ECommerce.Application.Models;

namespace ECommerce.Application.Interfaces;

public interface IResetPasswordTokenService
{
    Task CreateAsync(ResetPasswordToken token);
    Task<ResetPasswordToken?> GetByTokenAsync(string token);
    Task DeleteAsync(ResetPasswordToken token);
}