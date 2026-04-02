using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;

namespace ECommerce.Infrastructure.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly ISqlDataAccess _db;

    public ShoppingCartRepository(ISqlDataAccess db)
    {
        _db = db;
    }

    public async Task<ShoppingCart?> GetItemAsync(string userId, int productId)
    {
        const string sql = """

            SELECT 
                Id, UserId, ProductId, Count
            FROM
                ShoppingCart
            WHERE
                UserId = @UserId AND ProductId = @ProductId

        """;

        return await _db.QuerySingleOrDefaultAsync<ShoppingCart, dynamic>
                         (sql, new { UserId = userId, ProductId = productId });
    }

    public async Task<bool> ClearCartAsync(string? userId)
    {
        const string sql = """

            DELETE 
            FROM
                ShoppingCart
            WHERE
                UserId = @UserId;

        """;

        int rows = await _db.ExecuteAsync(sql, new { UserId = userId });
        return rows > 0;
    }

    public async Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId)
    {
        const string sql = """
            SELECT 
                sc.Id, sc.UserId, sc.ProductId, sc.Count,
                p.Id, p.Name, p.Price, p.Description, p.SpecialTag, p.ImageUrl, p.CategoryId,
                c.Id, c.Name
            FROM 
                ShoppingCart sc
            INNER JOIN 
                Product p 
            ON 
                sc.ProductId = p.Id
            JOIN 
                Category c 
            ON 
                p.CategoryId = c.Id
            WHERE 
                sc.UserId = @UserId
        """;

        return await _db.QueryAsync<ShoppingCart, Product, Category, ShoppingCart, dynamic>(
            sql,
            (cart, product, category) =>
            {
                product.Category = category;
                cart.Product = product;
                return cart;
            },
            new { UserId = userId }
        );
    }

    public Task<int> GetTotalCartCountAsync(string? userId)
    {
        const string sql = """

            SELECT 
                COALESCE(SUM(Count), 0)
            FROM
                ShoppingCart
            WHERE
                UserId = @UserId

        """;
        
        return _db.ExecuteScalarAsync<int, dynamic>(sql, new { UserId = userId });
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = """

            DELETE 
            FROM 
                ShoppingCart
            WHERE 
                Id = @Id

        """;

        int rows = await _db.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }

    public async Task<bool> UpdateCartAsync(string userId, int productId, int updateBy)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return false;
        }

        var existing = await GetItemAsync(userId, productId);

        if(existing is not null)
        {   
            const string updateSql = """
                
                UPDATE 
                    ShoppingCart
                SET 
                    Count = @Count
                WHERE 
                    Id = @Id
            
            """;

            int rowsUpdated = await _db.ExecuteAsync(updateSql, new { Id = existing.Id, Count = newCount });
            return rowsUpdated > 0;
        }

        const string insertSql = """

            INSERT INTO
                ShoppingCart (UserId, ProductId, Count)
            VALUES 
                (@UserId, @ProductId, @Count)

        """;

        int rowInserted = await _db.ExecuteAsync(insertSql,
            new
            {
                UserId = userId,
                ProductId = productId,
                Count = updateBy
            });

        return rowInserted > 0;
    }
}