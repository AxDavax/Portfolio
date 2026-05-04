using ECommerce.ClientPortal.ViewModels.Core;
using System.Security.Claims;

namespace ECommerce.ClientPortal.ViewModels.Auth;

public class ProfileVM : BaseVM
{
    private readonly AuthUserVM _authUser;

    public string Email => _authUser.User?.FindFirst("email")?.Value ?? string.Empty;
    public string Name => _authUser.User?.FindFirst("name")?.Value ?? string.Empty;
    public string UserId => _authUser.User?.FindFirst("uid")?.Value ?? string.Empty;
    public string FirstName => _authUser.User?.FindFirst("firstName")?.Value ?? string.Empty;
    public string LastName => _authUser.User?.FindFirst("lastName")?.Value ?? string.Empty;


    public IEnumerable<Claim> Claims => _authUser.User?.Claims ?? Enumerable.Empty<Claim>();


    public ProfileVM(AuthUserVM authUser)
    {
        _authUser = authUser;

        // when AuthUserVM changes, ProfileVM updates automatically
        _authUser.StateChanged += OnAuthUserChanged;
    }

    private void OnAuthUserChanged()
    {
        NotifyStateChanged();
    }

    public async Task LoadAsync()
    {
        await _authUser.WaitUntilReadyAsync();
        NotifyStateChanged();
    }
}