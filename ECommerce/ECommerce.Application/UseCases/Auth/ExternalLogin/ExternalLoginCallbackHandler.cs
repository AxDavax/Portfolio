using ECommerce.Application.Interfaces;
using MediatR;

namespace ECommerce.Application.UseCases.Auth.ExternalLogin
{
    public record ExternalLoginCallbackRequest(string Provider, string Code, string State)
        : IRequest<ExternalLoginCallbackResponse>;

    public record ExternalLoginCallbackResponse(
        bool Success,
        string? Email,
        string? ProviderUserId,
        string? ErrorMessage);

    public class ExternalLoginCallbackHandler : IRequestHandler<ExternalLoginCallbackRequest, ExternalLoginCallbackResponse>
    {
        private readonly IExternalLoginStateStore _stateStore;
        private readonly IEnumerable<IExternalAuthService> _authServices;

        public ExternalLoginCallbackHandler(
            IExternalLoginStateStore stateStore,
            IEnumerable<IExternalAuthService> authServices)
        {
            _stateStore = stateStore;
            _authServices = authServices;
        }

        public async Task<ExternalLoginCallbackResponse> Handle(
            ExternalLoginCallbackRequest request,
            CancellationToken cancellationToken)
        {
            // 1. Validate state
            var isValidState = await _stateStore.ValidateAsync(request.State, request.Provider);
            if (!isValidState)
            {
                return new ExternalLoginCallbackResponse(
                    Success: false,
                    Email: null,
                    ProviderUserId: null,
                    ErrorMessage: "Invalid or expired OAuth state.");
            }

            // 2. Resolve provider service
            var service = _authServices.FirstOrDefault(s =>
                s.GetType().Name.StartsWith(request.Provider, StringComparison.OrdinalIgnoreCase));

            if (service == null)
            {
                return new ExternalLoginCallbackResponse(
                    Success: false,
                    Email: null,
                    ProviderUserId: null,
                    ErrorMessage: $"Unknown external provider: {request.Provider}");
            }

            // 3. Exchange code for user info
            var userInfo = await service.GetUserInfoAsync(request.Code);

            if (userInfo == null)
            {
                return new ExternalLoginCallbackResponse(
                    Success: false,
                    Email: null,
                    ProviderUserId: null,
                    ErrorMessage: "Failed to retrieve external user information.");
            }

            // 4. Success
            return new ExternalLoginCallbackResponse(
                Success: true,
                Email: userInfo.Email,
                ProviderUserId: userInfo.ProviderId,
                ErrorMessage: null);
        }
    }
}