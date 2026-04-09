using ECommerce.ClientPortal.ViewModels.Core;
using System.Security.Claims;

namespace ECommerce.ClientPortal.ViewModels.Auth;

public class ProfileVM : BaseVM
{
    private readonly AuthUserVM _authUser;

    public string Email => _authUser.User?.Identity?.Name ?? string.Empty;
    public string FirstName => _authUser.User?.FindFirst("firstName")?.Value ?? string.Empty;
    public string LastName => _authUser.User?.FindFirst("lastname")?.Value ?? String.Empty;

    public IEnumerable<Claim> Claims => _authUser.User?.Claims ?? Enumerable.Empty<Claim>();


    public ProfileVM(AuthUserVM authUser)
    {
        _authUser = authUser;

        // when AuthUserVM changes, ProfileVM updates automatically
        _authUser.PropertyChanged += (_, __) => OnPropertyChanged(null);
    }

    public async Task LoadAsync()
    {
        if(!_authUser.IsReady)
            await _authUser.LoadAsync();

        OnPropertyChanged(null);
    }
}