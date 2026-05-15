using ECommerce.Application.OAuth.Interfaces;
using ECommerce.Application.OAuth.Records;
using MediatR;

namespace ECommerce.Application.OAuth.UseCases;

public class StartExternalLoginHandler
    : IRequestHandler<StartExternalLoginRequest, StartExternalLoginResponse>
{
    private readonly IExternalLoginStateStore _stateStore;
    private readonly IEnumerable<IExternalAuthService> _authServices;

    public StartExternalLoginHandler(
        IExternalLoginStateStore stateStore,
        IEnumerable<IExternalAuthService> authServices)
    {
        _stateStore = stateStore;
        _authServices = authServices;
    }

    public async Task<StartExternalLoginResponse> Handle(
        StartExternalLoginRequest request,
        CancellationToken cancellationToken)
    {
        var service = _authServices.FirstOrDefault(s =>
            s.GetType().Name.StartsWith(request.Provider, StringComparison.OrdinalIgnoreCase));

        if (service == null)
            throw new InvalidOperationException($"Unknown external provider: {request.Provider}");

        var state = Guid.NewGuid().ToString("N");

        await _stateStore.StoreAsync(state, request.Provider);

        var url = service.GetAuthorizationUrl(state);

        return new StartExternalLoginResponse(url);
    }
}