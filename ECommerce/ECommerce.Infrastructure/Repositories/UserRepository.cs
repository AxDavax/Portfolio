using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;
using ECommerce.Application.Models;

namespace ECommerce.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ISqlDataAccess _db;

    public UserRepository(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task CreateAsync(User user)
    {
        const string sql = """

            INSERT INTO 
                Users (Id, Email, FirstName, LastName, IsActive, CreatedAt)
            VALUES 
                (@Id, @Email, @FirstName, @LastName, @IsActive, GETUTCDATE())

        """;

        return _db.ExecuteAsync(sql, new
        {
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.IsActive
        });
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        const string sql = """

            SELECT
                1
            FROM
                Users
            WHERE
                Email = @Email

        """;

        var result = await _db.QuerySingleOrDefaultAsync<int?, dynamic>(
            sql, new { Email = email });

        return result.HasValue;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        const string sql = """

            SELECT 
                *
            FROM 
                Users
            WHERE
                Email = @Email

        """;

        var sqlUser = await _db.QuerySingleOrDefaultAsync<SqlUser, dynamic>(
            sql, new { Email = email });

        return sqlUser == null ? null : MapToDomain(sqlUser);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        const string sql = """

            SELECT 
                *
            FROM 
                Users
            WHERE
                Id = @Id

        """;

        var sqlUser = await _db.QuerySingleOrDefaultAsync<SqlUser, dynamic>(
            sql, new { Id = id });

        return sqlUser == null ? null : MapToDomain(sqlUser);
    }

    public Task<IEnumerable<string>> GetRolesAsync(Guid userId)
    {
        const string sql = """
            
            SELECT 
                R.Name
            FROM 
                UserRoles UR
            JOIN 
                Roles R 
            ON 
                UR.RoleId = R.Id
            WHERE 
                UR.UserId = @UserId
            
        """;

        return _db.QueryAsync<string, dynamic>(sql, new { UserId = userId });
    }

    private static User MapToDomain(SqlUser sqlUser)
    {
        return new User
        {
            Id = sqlUser.Id,
            Email = sqlUser.Email,
            FirstName = sqlUser.FirstName,
            LastName = sqlUser.LastName,
            IsActive = sqlUser.IsActive
        };
    }
}