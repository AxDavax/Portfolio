namespace ECommerce.Domain.Interfaces;

public interface ISqlDataAccess
{
    Task<IEnumerable<T>> QueryAsync<T, U>(string sql, U parameters);
    Task<T?> QuerySingleAsync<T, U>(string sql, U parameters);
    Task<int> ExecuteAsync<T>(string sql, T parameters);
    Task<T> ExecuteScalarAsync<T, U>(string sql, U parameters);
}