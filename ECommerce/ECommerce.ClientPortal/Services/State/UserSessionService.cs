using ECommerce.ClientPortal.Models;

namespace ECommerce.ClientPortal.Services.State;

public class UserSessionService
{
    public UserSession? Current { get; private set; }
    
    public void Set(UserSession session)
    {
        Current = session;
    }

    public void Clear()
    {
        Current = null;
    }
}