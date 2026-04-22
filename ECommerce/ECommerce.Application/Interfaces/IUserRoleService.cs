namespace ECommerce.Application.Interfaces;

public interface IUserRoleService
{
    Task AssignRoleAsync(Guid userId, Guid roleId);
}