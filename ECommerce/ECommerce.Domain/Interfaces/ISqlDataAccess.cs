namespace ECommerce.Domain.Interfaces;

public interface ISqlDataAccess
{
    Task<IEnumerable<T>> QueryAsync<T, U>(string sql, U parameters);
    Task<T?> QuerySingleOrDefaultAsync<T, U>(string sql, U parameters);
    Task<int> ExecuteAsync<T>(string sql, T parameters);
    Task<T> ExecuteScalarAsync<T, U>(string sql, U parameters);
    Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn, U>(
        string sql, 
        Func<TFirst, TSecond, TReturn> map, 
        U parameters, 
        string splitOn = "Id"
    );
    Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn, U>(
        string sql,
        Func<TFirst, TSecond, TThird, TReturn> map,
        U parameters,
        string splitOn = "Id"
    );
}