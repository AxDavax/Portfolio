using ECommerce.Application.OAuth.Interfaces;
using ECommerce.Application.OAuth.Records;
using MediatR;

namespace ECommerce.Application.OAuth.UseCases;

public class StartExternalLoginHandler
    : IRequestHandler<StartExternalLoginRequest, StartExternalLoginResponse>
{
    private readonly IExternalLoginStateStore _stateStore;
    private readonly IEnumerable<IExternalAuthProvider> _authProvider;

    public StartExternalLoginHandler(
        IExternalLoginStateStore stateStore,
        IEnumerable<IExternalAuthProvider> authProvider)
    {
        _stateStore = stateStore;
        _authProvider = authProvider;
    }

    public async Task<StartExternalLoginResponse> Handle(
        StartExternalLoginRequest request,
        CancellationToken cancellationToken)
    {
        var provider = _authProvider.FirstOrDefault(s =>
            s.GetType().Name.StartsWith(request.Provider, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
            throw new InvalidOperationException($"Unknown external provider: {request.Provider}");

        var state = Guid.NewGuid().ToString("N");

        await _stateStore.StoreAsync(state, request.Provider);

        var url = provider.GetAuthorizationUrl(state);

        return new StartExternalLoginResponse(url);
    }
}