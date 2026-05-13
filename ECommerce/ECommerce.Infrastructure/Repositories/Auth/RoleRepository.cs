using ECommerce.Domain.Auth.Interfaces;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Infrastructure.Repositories.Auth;

public class RoleRepository : IRoleRepository
{
    private readonly ISqlDataAccess _db;
    
    public RoleRepository(ISqlDataAccess db)
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

        return _db.ExecuteScalarAsync<Guid, object>(sql, new { Name = roleName });
    }
}