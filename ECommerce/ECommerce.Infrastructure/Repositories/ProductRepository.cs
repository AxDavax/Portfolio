
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;

namespace ECommerce.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ISqlDataAccess _db;

    public ProductRepository(ISqlDataAccess db)
    {
        _db = db;
    }

    public async Task<Product> CreateAsync(Product obj)
    {
        const string sql = """    

            INSERT INTO Product (Name, Price, Description, SpecialTag, CategoryId, ImageUrl)
            VALUES (@Name, @Price, @Description, @SpecialTag, @CategoryId, @ImageUrl);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
        
            """;

        int newId = await _db.ExecuteScalarAsync<int, dynamic>(sql, new
        {
            obj.Name,
            obj.Price,
            obj.Description,
            obj.SpecialTag,
            obj.CategoryId,
            obj.ImageUrl
        });

        return new Product
        {
            Id = newId,
            Name = obj.Name,
            Price = obj.Price,
            Description = obj.Description,
            SpecialTag = obj.SpecialTag,
            CategoryId = obj.CategoryId,
            ImageUrl = obj.ImageUrl
        };
    }

    
    public Task<IEnumerable<Product>> GetAllAsync()
    {
        const string sql = """

            SELECT 
                Id, Name, Price, Description, SpecialFlag, ImageUrl, CategoryId
            FROM 
                Product 
               
            """;

        return _db.QueryAsync<Product, dynamic>(sql, new { });
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        const string sql = """
             
            SELECT 
                Id, Name, Price, Description, SpecialFlag, CategoryId, ImageUrl
            FROM 
                Product 
            WHERE 
                Id = @Id
                        
            """;

        return _db.QuerySingleOrDefaultAsync<Product, dynamic>(sql, new { Id = id });
    }

    public async Task<Product> UpdateAsync(Product obj)
    {
        const string sql = """

            UPDATE Product
            SET Name = @Name,
                Price = @Price,
                Description = @Description,
                SpecialTag = @SpecialTag,
                CategoryId = @CategoryId,
                ImageUrl = @ImageUrl
            WHERE Id = @Id

            """;

        int rows = await _db.ExecuteAsync(sql, obj);

        if(rows == 0) throw new KeyNotFoundException($"Product with Id {obj.Id} not found.");

        return obj;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = """

            DELETE 
            FROM Product
            WHERE Id = @Id

            """;

        int rows = await _db.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}