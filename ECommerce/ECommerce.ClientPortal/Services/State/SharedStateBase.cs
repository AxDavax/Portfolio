namespace ECommerce.ClientPortal.Services.State;

public abstract class SharedStateBase
{
    public event Action OnChange;

    protected void NotifyStateChanged() => OnChange?.Invoke();
}