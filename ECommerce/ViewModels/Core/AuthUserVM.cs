using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Portfolio.ECommerce.Blazor.ViewModels.Core
{
    public class AuthUserVM : BaseVM
    {
        private readonly AuthenticationStateProvider _authStateProvider;

        public ClaimsPrincipal User { get; private set; }

        public AuthUserVM(AuthenticationStateProvider authenticationStateProvider) 
        {
            _authStateProvider = authenticationStateProvider;
        }

        public async Task LoadAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            User = authState.User;
        }
    }
}