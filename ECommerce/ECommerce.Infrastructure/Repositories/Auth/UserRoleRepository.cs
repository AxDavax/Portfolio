using ECommerce.Domain.Auth.Interfaces;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Infrastructure.Repositories.Auth;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly ISqlDataAccess _db;

    public UserRoleRepository(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task AssignRoleAsync(Guid userId, Guid roleId)
    {
        const string sql = """
            
            INSERT INTO 
                UserRoles (UserId, RoleId)
            VALUES 
                (@UserId, @RoleId)
            
        """;

        return _db.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId });
    }
}