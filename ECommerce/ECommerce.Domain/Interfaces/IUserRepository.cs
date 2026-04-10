using ECommerce.Domain.Models;

namespace ECommerce.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<IEnumerable<string>> GetRolesAsync(Guid userId);
    Task<bool> EmailExistsAsync(string email);
    Task CreateAsync(User user);
    Task UpdatePasswordAsync(Guid userId, string hash, string salt);
}