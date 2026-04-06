using ECommerce.Application.Models;

namespace ECommerce.Application.Interfaces;

public interface IUserAuthRepository
{
    Task<SqlUser?> GetSqlUserByEmailAsync(string email);
}