using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Infrastructure.Auth
{
    public class UserRoleService : IUserRoleService
    {
        private readonly ISqlDataAccess _db;

        public UserRoleService(ISqlDataAccess db)
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
}