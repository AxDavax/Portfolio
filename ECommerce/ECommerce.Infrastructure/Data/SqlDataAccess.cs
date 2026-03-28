using Dapper;
using ECommerce.Domain.Interfaces;
using System.Data;

namespace ECommerce.Infrastructure.Data;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly DapperContext _context;

    public SqlDataAccess(DapperContext context)
    {
        _context = context;
    }

    public async Task<int> ExecuteAsync<T>(string sql, T parameters)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.ExecuteAsync(sql, parameters);
    }

    public async Task<T> ExecuteScalarAsync<T, U>(string sql, U parameters)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.ExecuteScalarAsync<T>(sql, parameters);
    }

    public async Task<IEnumerable<T>> QueryAsync<T, U>(string sql, U parameters)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryAsync<T>(sql, parameters);
    }

    public async Task<T?> QuerySingleAsync<T, U>(string sql, U parameters)
    {
        using IDbConnection connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }
}