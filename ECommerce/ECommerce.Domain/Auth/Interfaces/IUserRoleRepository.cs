namespace ECommerce.Domain.Auth.Interfaces;

public interface IUserRoleRepository
{
    Task AssignRoleAsync(Guid userId, Guid roleId);
}