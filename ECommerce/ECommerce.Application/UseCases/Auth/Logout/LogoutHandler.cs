using ECommerce.Contracts.Auth.Logout;
using ECommerce.Domain.Auth.Interfaces;

namespace ECommerce.Application.UseCases.Auth.Logout
{
    public class LogoutHandler
    {
        private readonly IRefreshTokenRepository _refreshTokenRepo;

        public LogoutHandler(IRefreshTokenRepository refreshTokenRepo)
        {
            _refreshTokenRepo = refreshTokenRepo;
        }

        public async Task<LogoutResponse> HandleAsync(LogoutRequest request)
        {
            // 1. Verify if the token exists
            var existing = await _refreshTokenRepo.GetRefreshTokenAsync(request.RefreshToken);
            if (existing != null)
                return new LogoutResponse { Success = false };

            // 2. Deletes the refresh token
            await _refreshTokenRepo.DeleteRefreshTokenAsync(request.RefreshToken);

            // 3. Returns Success
            return new LogoutResponse { Success = true };
        }
    }
}