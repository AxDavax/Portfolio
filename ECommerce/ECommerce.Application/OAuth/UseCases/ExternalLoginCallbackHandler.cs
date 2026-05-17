using ECommerce.Application.OAuth.Interfaces;
using ECommerce.Application.OAuth.Records;
using ECommerce.Contracts.OAuth.Models;
using MediatR;

namespace ECommerce.Application.OAuth.UseCases;

public class ExternalLoginCallbackHandler : IRequestHandler<ExternalLoginCallbackRequest, ExternalLoginCallbackResponse>
{
    private readonly IExternalLoginStateStore _stateStore;
    private readonly IEnumerable<IExternalAuthProvider> _authProvider;
    private readonly IExternalLoginService _externalLoginService;

    public ExternalLoginCallbackHandler(
        IExternalLoginStateStore stateStore,
        IEnumerable<IExternalAuthProvider> authProvider,
        IExternalLoginService externalLoginService)
    {
        _stateStore = stateStore;
        _authProvider = authProvider;
        _externalLoginService = externalLoginService;
    }

    public async Task<ExternalLoginCallbackResponse> Handle(
        ExternalLoginCallbackRequest request,
        CancellationToken cancellationToken)
    {
        // 1. Validate state
        var isValidState = await _stateStore.ValidateAsync(request.State, request.Provider);
        if (!isValidState)
        {
            return new ExternalLoginCallbackResponse
            {
                Success = false,
                Email = null,
                ProviderUserId = null,
                ErrorMessage = "Invalid or expired OAuth state."
            };
        }

        // 2. Resolve provider service
        var provider = _authProvider.FirstOrDefault(s =>
            s.GetType().Name.StartsWith(request.Provider, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return new ExternalLoginCallbackResponse
            {
                Success = false,
                Email = null,
                ProviderUserId = null,
                ErrorMessage = $"Unknown external provider: {request.Provider}"
            };
        }

        // 3. Exchange code for user info
        var userInfo = await provider.GetUserInfoAsync(request.Code);

        if (userInfo == null)
        {
            return new ExternalLoginCallbackResponse
            {
                Success = false,
                Email = null,
                ProviderUserId = null,
                ErrorMessage = "Failed to retrieve external user information."
            };
        }

        var authResult = await _externalLoginService.HandleExternalUserAsync(
            provider: request.Provider,
            providerUserId: userInfo.ProviderId,
            email: userInfo.Email);

        // 4. Success
        return new ExternalLoginCallbackResponse
        {
            Success = authResult.Success,
            Email = authResult.Email,
            ProviderUserId = authResult.ProviderUserId,
            ErrorMessage = authResult.ErrorMessage
        };
    }
}