using AutoMapper;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;

namespace ECommerce.Infrastructure.Repositories;

public class UserRepository : IUserRepository, IUserAuthRepository
{
    private readonly ISqlDataAccess _db;
    private readonly IMapper _mapper;

    public UserRepository(ISqlDataAccess db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public Task CreateAsync(User user)
    {
        const string sql = """

            INSERT INTO 
                Users (Id, Email, FirstName, LastName, IsActive)
            VALUES 
                (@Id, @Email, @FirstName, @LastName, @IsActivea)

        """;

        return _db.ExecuteAsync(sql, user);
    }

    public async Task<User> CreateAuthAsync(SqlUser user)
    {
        const string sql = """
            
            INSERT INTO Users 
                (Id, Email, FirstName, LastName, PasswordHash, PasswordSalt, IsActive)
            VALUES 
                (@Id, @Email, @FirstName, @LastName, @PasswordHash, @PasswordSalt, @IsActive);
            
        """;

        await _db.ExecuteAsync(sql, user);

        return _mapper.Map<User>(user);
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

        var user = await _db.QuerySingleOrDefaultAsync<User, dynamic>(
            sql, new { Email = email });

        return user ?? null;
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

        var user = await _db.QuerySingleOrDefaultAsync<User, dynamic>(
            sql, new { Id = id });

        return user ?? null;
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

    public async Task<SqlUser?> GetSqlUserByEmailAsync(string email)
    {
        const string sql = """

            SELECT
                *
            FROM
                Users
            WHERE
                Email = @Email

        """;

        return await _db.QuerySingleOrDefaultAsync<SqlUser, dynamic>(
            sql, new { Email = email });
    }
}