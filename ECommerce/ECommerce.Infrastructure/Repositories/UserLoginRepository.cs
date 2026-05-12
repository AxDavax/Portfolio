using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;

namespace ECommerce.Infrastructure.Repositories;

public class UserLoginRepository : IUserLoginRepository
{
    private readonly ISqlDataAccess _db;

    public UserLoginRepository(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task<UserLogin?> GetByProviderAsync(string provider, string providerUserId)
    {
        const string sql = """
             
            SELECT 
                * 
            FROM 
                UserLogin 
            WHERE 
                Provider = @Provider AND ProviderUserId = @ProviderUserId
                        
        """;
        
        return _db.QuerySingleOrDefaultAsync<UserLogin, object>(sql, 
            new { 
                Provider = provider, 
                ProviderUserId = providerUserId 
            });
    }

    public Task<IEnumerable<UserLogin>> GetByUserIdAsync(Guid userId)
    {
        const string sql = """
             
            SELECT 
                * 
            FROM 
                UserLogin 
            WHERE 
                UserId = @UserId
                        
        """;
        
        return _db.QueryAsync<UserLogin, object>(sql, new { UserId = userId });
    }

    public Task AddAsync(UserLogin login)
    {
        const string sql = """
             
            INSERT INTO UserLogin (Id, UserId, Provider, ProviderUserId, CreatedAt) 
            VALUES (@Id, @UserId, @Provider, @ProviderUserId, @CreatedAt)
                        
        """;
        
        return _db.ExecuteAsync(sql, login);
    }
}