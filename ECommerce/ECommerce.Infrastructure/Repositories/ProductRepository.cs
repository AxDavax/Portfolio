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

        obj.Id = newId;
        return obj;
    }

    
    public Task<IEnumerable<Product>> GetAllAsync()
    {
        const string sql = """

            SELECT 
                p.Id, p.Name, p.Price, p.Description, p.SpecialTag, p.ImageUrl, p.CategoryId,
                c.Id, c.Name
            FROM 
                Product p
            INNER JOIN
                Category c
            ON
                p.CategoryId = c.Id
               
            """;

        return _db.QueryAsync<Product, Category, Product, dynamic>(
            sql, 
            (product, category) =>
            {
                product.Category = category;
                return product;
            },
            new { }
        );
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        const string sql = """
             
            SELECT 
                p.Id, p.Name, p.Price, p.Description, p.SpecialTag, p.CategoryId, p.ImageUrl,
                c.Id, c.Name
            FROM 
                Product p
            INNER JOIN 
                Category c
            ON
                p.CategoryId = c.Id
            WHERE 
                Id = @Id
                        
            """;

        var result = await _db.QueryAsync<Product, Category, Product, dynamic>(
            sql,
            (product, category) =>
            {
                product.Category = category;
                return product;
            },
            new { Id = id }
        );

        return result.FirstOrDefault();
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


        return await GetByIdAsync(obj.Id) ?? 
            throw new Exception("Unexpected error: product updated but not found.");
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