namespace ECommerce.Domain.Interfaces;

public interface IRoleRepository
{
    Task<Guid> GetIdByNameAsync(string roleName);
}