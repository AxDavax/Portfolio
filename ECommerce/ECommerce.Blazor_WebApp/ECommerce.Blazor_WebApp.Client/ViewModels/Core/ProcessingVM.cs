namespace ECommerce.Blazor_WebApp.Client.ViewModels.Core;

public abstract class ProcessingVM : BaseVM
{
    private bool _isProcessing = false;
    public bool IsProcessing
    {
        get => _isProcessing;
        set => SetProperty(ref _isProcessing, value);
    }
}