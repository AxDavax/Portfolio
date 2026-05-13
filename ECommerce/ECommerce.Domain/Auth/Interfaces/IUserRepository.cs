using ECommerce.Domain.Auth.Models;

namespace ECommerce.Domain.Auth.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<IEnumerable<string>> GetRolesAsync(Guid userId);
    Task<bool> EmailExistsAsync(string email);
    Task CreateAsync(User user);
    Task UpdatePasswordAsync(Guid userId, string hash, string salt);
}