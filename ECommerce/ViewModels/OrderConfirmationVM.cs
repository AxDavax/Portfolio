using Portfolio.ECommerce.Blazor.Data;
using Portfolio.ECommerce.Blazor.Repository.IRepository;
using Portfolio.ECommerce.Blazor.Services;
using Portfolio.ECommerce.Blazor.Utility;

namespace Portfolio.ECommerce.Blazor.ViewModels
{
    public class OrderConfirmationVM : BaseVM
    {
        private readonly PaymentService _paymentService;
        private readonly IShoppingCartRepository _cartRepository;

        public OrderConfirmationVM(PaymentService paymentService, IShoppingCartRepository cartRepository)
        {
            _paymentService = paymentService;
            _cartRepository = cartRepository;
        }

        private string _sessionId = string.Empty;
        public string SessionId 
        { 
            get => _sessionId; 
            set => SetProperty(ref _sessionId, value);
        }

        private bool _isProcessing = true;
        public bool IsProcessing
        {
            get => _isProcessing;
            set => SetProperty(ref _isProcessing, value);
        }

        private OrderHeader _orderHeader = new();
        public OrderHeader OrderHeader
        {
            get => _orderHeader;
            set => SetProperty(ref _orderHeader, value);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task AfterRenderAsync(bool firstRender)
        {
            if (firstRender) await LoadOrderAsync();
        }

        public async Task LoadOrderAsync()
        {
            await RunCommandAsync(() => IsProcessing, async () =>
            {
                OrderHeader = await _paymentService.CheckPaymentStatusAndUpdateOrder(SessionId);

                if (OrderHeader.Status == SD.StatusApproved)
                {
                    await _cartRepository.ClearCartAsync(OrderHeader.UserId);
                }
            });
        }
    }
}