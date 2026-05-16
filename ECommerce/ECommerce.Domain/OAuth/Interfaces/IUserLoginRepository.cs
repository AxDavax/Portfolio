using ECommerce.Domain.Auth.Models;

namespace ECommerce.Domain.OAuth.Interfaces;

public interface IUserLoginRepository
{
    Task<UserLogin?> GetByProviderAsync(string provider, string providerUserId);
    Task<IEnumerable<UserLogin>> GetByUserIdAsync(Guid userId);
    Task AddAsync(UserLogin login);
}