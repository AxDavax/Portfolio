namespace ECommerce.ClientPortal.Services.State;

public class CartState
{
    public event Action? StateChanged;

    private void NotifyStateChanged() => StateChanged?.Invoke();
    
    private int _count;

    public int Count
    {
        get => _count;
        private set
        {
            if (_count != value)
            {
                _count = value;
                NotifyStateChanged();
            }
        }
    }

    public void SetCount(int value) => Count = value;

    public void Increment() => Count++;

    public void Reset() => Count = 0;
}