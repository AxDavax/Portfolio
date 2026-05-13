namespace ECommerce.Domain.Auth.Interfaces;

public interface IRoleRepository
{
    Task<Guid> GetIdByNameAsync(string roleName);
}