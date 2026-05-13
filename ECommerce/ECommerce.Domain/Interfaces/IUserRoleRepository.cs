namespace ECommerce.Domain.Interfaces;

public interface IUserRoleRepository
{
    Task AssignRoleAsync(Guid userId, Guid roleId);
}