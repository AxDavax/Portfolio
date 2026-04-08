using ECommerce.Application.Interfaces;

namespace ECommerce.Application.UseCases.Auth.Logout
{
    public class LogoutHandler
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public LogoutHandler(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        public async Task<LogoutResponse> HandleAsync(LogoutRequest request)
        {
            // 1. Verify if the token exists
            var existing = await _refreshTokenService.GetRefreshTokenAsync(request.RefreshToken);
            if (existing != null)
                return new LogoutResponse { Success = false };

            // 2. Deletes the refresh token
            await _refreshTokenService.DeleteRefreshTokenAsync(request.RefreshToken);

            // 3. Returns Success
            return new LogoutResponse { Success = true };
        }
    }
}