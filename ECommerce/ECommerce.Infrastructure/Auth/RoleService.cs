using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Infrastructure.Auth
{
    public class RoleService : IRoleService
    {
        private readonly ISqlDataAccess _db;

        public RoleService(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<Guid> GetIdByNameAsync(string roleName)
        {
            const string sql = """    
                
                SELECT 
                    Id 
                FROM 
                    Roles 
                WHERE 
                    Name = @Name
            
            """;

            return _db.ExecuteScalarAsync<Guid, dynamic>(sql, new { Name = roleName });
        }
    }
}