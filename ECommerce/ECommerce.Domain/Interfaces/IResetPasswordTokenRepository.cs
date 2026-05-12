using ECommerce.Domain.Models;

namespace ECommerce.Domain.Interfaces;

public interface IResetPasswordTokenRepository
{
    Task CreateAsync(ResetPasswordToken token);
    Task<ResetPasswordToken?> GetByTokenAsync(string token);
    Task DeleteAsync(ResetPasswordToken token);
}