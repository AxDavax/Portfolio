using Microsoft.AspNetCore.Components;
using Portfolio.ECommerce.Blazor.Data;
using Portfolio.ECommerce.Blazor.Repository.IRepository;
using Portfolio.ECommerce.Blazor.Services;
using Portfolio.ECommerce.Blazor.Utility;
using Portfolio.ECommerce.Blazor.ViewModels.Core;

namespace Portfolio.ECommerce.Blazor.ViewModels.Cart
{
    public class CartVM : ProcessingVM
    {
        private readonly IShoppingCartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly NavigationManager _navigation;
        private readonly SharedStateService _sharedStateService;
        private readonly PaymentService _paymentService;
        private readonly AuthUserVM _authUser;

        public CartVM(IShoppingCartRepository cartRepository, 
                      IOrderRepository orderRepository, 
                      NavigationManager navigation, 
                      SharedStateService sharedStateService, 
                      PaymentService paymentService,
                      AuthUserVM authUser)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _navigation = navigation;
            _sharedStateService = sharedStateService;
            _paymentService = paymentService;
            _authUser = authUser;
        }

        private IEnumerable<ShoppingCart> _shoppingCarts = new List<ShoppingCart>();
        public IEnumerable<ShoppingCart> ShoppingCarts
        {
            get => _shoppingCarts;
            set => SetProperty(ref _shoppingCarts, value);
        }

        private OrderHeader _orderHeader = new();
        public OrderHeader OrderHeader
        {
            get => _orderHeader;
            set => SetProperty(ref _orderHeader, value);
        }

        private int _totalItems;
        public int TotalItems
        {
            get => _totalItems;
            set => SetProperty(ref _totalItems, value);
        }

        public async Task InitializeAsync()
        {
            await Task.Delay(1000);
            var user = _authUser.User;

            if (user is null) return;
            
            OrderHeader.Email = user.FindFirst(u => u.Type.Contains("email"))?.Value;
            OrderHeader.UserId = user.FindFirst(u => u.Type.Contains("nameidentifier"))?.Value;
            OrderHeader.Status = SD.StatusPending;
        }

        public async Task AfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadCartAsync();
            }
        }

        public async Task LoadCartAsync()
        {
            await RunCommandAsync(() => IsProcessing, async () =>
            {
                ShoppingCarts = await _cartRepository.GetAllAsync(OrderHeader.UserId);
                RecalculateTotals();
            });
        }

        private void RecalculateTotals()
        {
            OrderHeader.OrderTotal = 0;
            TotalItems = 0;

            foreach (var cart in ShoppingCarts)
            {
                OrderHeader.OrderTotal += Convert.ToDouble(cart.Product.Price) * cart.Count;
                TotalItems += cart.Count;
            }

            OrderHeader.OrderTotal = Math.Round(OrderHeader.OrderTotal, 2);
        }

        public async Task UpdateCartItemAsync(int productId, int updateBy)
        {
            await RunCommandAsync(() => IsProcessing, async () =>
            {
                var result = await _cartRepository.UpdateCartAsync(OrderHeader.UserId, productId, updateBy);
                _sharedStateService.TotalCartCount = await _cartRepository.GetTotalCartCountAsync(OrderHeader.UserId);
                await LoadCartAsync();
            });
        }

        public async Task ProcessOrderCreationAsync()
        {
            await RunCommandAsync(() => IsProcessing, async () =>
            {
                await Task.Yield();
                OrderHeader.OrderDetails = SD.ConvertShoppingCartListToOrderDetail(ShoppingCarts.ToList());

                var session = _paymentService.CreateStripeCheckoutSession(OrderHeader);
                OrderHeader.SessionId = session.Id;

                await _orderRepository.CreateAsync(OrderHeader);
                _navigation.NavigateTo(session.Url);
            });
        }
    }
}