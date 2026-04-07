using ECommerce.Application.Models;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Interfaces;

public interface IUserAuthRepository
{
    Task<SqlUser?> GetSqlUserByEmailAsync(string email);
    Task<User> CreateAuthAsync(SqlUser user);
}