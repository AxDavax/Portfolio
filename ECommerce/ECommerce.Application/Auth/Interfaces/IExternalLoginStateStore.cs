namespace ECommerce.Application.Auth.Interfaces;

public interface IExternalLoginStateStore
{
    Task StoreAsync(string state, string provider);
    Task<bool> ValidateAsync(string state, string provider);
    Task RemoveAsync(string state);
}