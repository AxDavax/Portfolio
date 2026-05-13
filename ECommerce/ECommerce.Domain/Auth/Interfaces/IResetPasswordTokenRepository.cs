using ECommerce.Domain.Auth.Models;

namespace ECommerce.Domain.Auth.Interfaces;

public interface IResetPasswordTokenRepository
{
    Task CreateAsync(ResetPasswordToken token);
    Task<ResetPasswordToken?> GetByTokenAsync(string token);
    Task DeleteAsync(ResetPasswordToken token);
}