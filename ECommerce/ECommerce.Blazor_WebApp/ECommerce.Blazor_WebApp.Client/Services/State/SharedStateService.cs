namespace ECommerce.Blazor_WebApp.Client.Services.State;

public class SharedStateService
{
    public event Action OnChange;
    private int _totalCartCount;

    public int TotalCartCount 
    {
        get => _totalCartCount;
        set { 
            _totalCartCount = value;
            NotifyStateChanged();
        } 
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}