using ECommerce.Application.Auth.Models;
using ECommerce.Domain.Auth.Models;

namespace ECommerce.Application.Auth.Interfaces;

public interface IUserAuthRepository
{
    Task<SqlUser?> GetSqlUserByEmailAsync(string email);
    Task<User> CreateAuthAsync(SqlUser user);
}