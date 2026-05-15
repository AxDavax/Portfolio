using ECommerce.Application.OAuth.Interfaces;
using System.Collections.Concurrent;

namespace ECommerce.Infrastructure.Auth;

public class ExternalLoginStateStore : IExternalLoginStateStore
{
    private readonly ConcurrentDictionary<string, string> _states = new();

    public Task StoreAsync(string state, string provider)
    {
        _states[state] = provider;
        return Task.CompletedTask;
    }

    public Task<bool> ValidateAsync(string state, string provider)
        => Task.FromResult(_states.TryGetValue(state, out var p) && p == provider);

    public Task RemoveAsync(string state)
    {
        _states.TryRemove(state, out _);
        return Task.CompletedTask;
    }
}