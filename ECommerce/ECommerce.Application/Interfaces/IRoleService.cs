namespace ECommerce.Application.Interfaces;

public interface IRoleService
{
    Task<Guid> GetIdByNameAsync(string roleName);
}