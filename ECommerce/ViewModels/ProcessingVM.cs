namespace Portfolio.ECommerce.Blazor.ViewModels
{
    public abstract class ProcessingVM : BaseVM
    {
        private bool _isProcessing = false;
        public bool IsProcessing
        {
            get => _isProcessing;
            set => SetProperty(ref _isProcessing, value);
        }
    }
}