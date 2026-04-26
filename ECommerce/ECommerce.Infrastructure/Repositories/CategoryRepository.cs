using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;

namespace ECommerce.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ISqlDataAccess _db;

    public CategoryRepository(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task<Category?> GetByIdAsync(int id)
    {
        const string sql = """
             
            SELECT Id, Name 
            FROM Category 
            WHERE Id = @Id
                        
            """;
        
        return _db.QuerySingleOrDefaultAsync<Category, object>(sql, new { Id = id });
    }

    public async Task<Category> CreateAsync(Category obj)
    {
        const string sql = """
            
            INSERT INTO Category (Name) 
            VALUES (@Name);    
            SELECT CAST(SCOPE_IDENTITY() AS INT)
          
            """;

        int newId = await _db.ExecuteScalarAsync<int, object>(sql, new { Name = obj.Name });

        return new Category
        {
            Id = newId,
            Name = obj.Name
        };
    }

    public async Task<Category> UpdateAsync(Category obj)
    {
        const string sql = """

            UPDATE Category
            SET Name = @Name
            WHERE Id = @Id

            """;

        int rows = await _db.ExecuteAsync(sql, obj);

        if(rows == 0) throw new KeyNotFoundException($"Category with Id {obj.Id} not found.");

        return obj;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = """
            
            DELETE 
            FROM Category 
            WHERE Id = @Id
            
            """;

        int rows = await _db.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }

    public Task<IEnumerable<Category>> GetAllAsync()
    {
        const string sql = "SELECT Id, Name FROM Category";
        return _db.QueryAsync<Category, object>(sql, null);
    }
}