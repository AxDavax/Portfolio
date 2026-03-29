using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;

namespace ECommerce.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ISqlDataAccess _db;

    public OrderRepository(ISqlDataAccess db)
    {
        _db = db;
    }

    public async Task<OrderHeader> CreateAsync(OrderHeader orderHeader)
    {
        orderHeader.OrderDate = DateTime.Now;

        const string sql = """

            INSERT INTO OrderHeader 
                (UserId, OrderTotal, OrderDate, Status, PhoneNumber, Email, Name, PaymentIntentId, SessionId)
            VALUES
                (@UserId, @OrderTotal, @OrderDate, @Status, @PhoneNumber, @Email, @Name, @PaymentIntentId, @SessionId)
            
            SELECT CAST(SCOPE_IDENTITY() AS INT);

        """;

        int newId = await _db.ExecuteScalarAsync<int, OrderHeader>(sql, orderHeader);
        orderHeader.Id = newId;

        return orderHeader;
    }

    public async Task<IEnumerable<OrderHeader>> GetAllAsync(string? userId = null)
    {
        string sql = """

            SELECT 
                *
            FROM 
                OrderHeader

        """;

        if (!string.IsNullOrEmpty(userId)) 
        {
            sql += " WHERE UserId = @UserId";
            return await _db.QueryAsync<OrderHeader, dynamic>(sql, new { UserId = userId });
        }

        return await _db.QueryAsync<OrderHeader, dynamic>(sql, new { });
    }

    public async Task<OrderHeader?> GetByIdAsync(int id)
    {
        const string sql = """

            SELECT
                oh.Id, oh.UserId, oh.OrderTotal, oh.OrderDate, oh.Status, oh.Name, oh.PhoneNumber, oh.Email, oh.SessionId, oh.PaymentIntentId,
                od.Id, od.OrderHeaderId, od.ProductId, od.Count, od.Price, od.ProductName
            FROM 
                OrderHeader oh
            LEFT JOIN
                OrderDetail od
            ON 
                oh.Id = od.OrderHeaderId
            WHERE
                oh.Id = @Id

        """;

        var lookup = new Dictionary<int, OrderHeader>();

        await _db.QueryAsync<OrderHeader, OrderDetail, OrderHeader, dynamic>(
            sql, 
            (header, detail) =>
            {
                if(!lookup.TryGetValue(header.Id, out var existing))
                {
                    existing = header;
                    existing.OrderDetails = new List<OrderDetail>();
                    lookup.Add(existing.Id, existing);
                }

                if(detail is not null) existing.OrderDetails.Add(detail);

                return existing;
            },
            new { Id = id }
        );

        return lookup.Values.FirstOrDefault();
    }

    public async Task<OrderHeader?> GetOrderBySessionIdAsync(string sessionId)
    {
        const string sql = """

            SELECT
                *
            FROM
                OrderHeader
            WHERE
                SessionId = @SessionId

        """;

        return await _db.QuerySingleOrDefaultAsync<OrderHeader, dynamic>(sql, new { SessionId = sessionId });
    }

    public async Task<OrderHeader?> UpdateStatusAsync(int orderId, string status, string paymentIntentId)
    {
        const string sql = """

            UPDATE
                OrderHeader
            SET
                Status = @Status,
                PaymentIntentId = COALESCE(@PaymentIntentId, PaymentIntentId)
            WHERE
                Id = @Id;


            SELECT
                *
            FROM
                OrderHeader
            WHERE
                ID = @Id;
            
        """;

        return await _db.QuerySingleOrDefaultAsync<OrderHeader, dynamic>(sql,
            new
            {
                Id = orderId,
                Status = status,
                paymentIntentId = paymentIntentId
            }
        );
    }
}