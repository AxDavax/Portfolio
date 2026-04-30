using ECommerce.ClientPortal.Services.API.Implementations;
using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.ClientPortal.Services.State;
using ECommerce.ClientPortal.Utility;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.DTO;
using Microsoft.AspNetCore.Components;

namespace Portfolio.ECommerce.Blazor.ViewModels.Cart;

public class CartVM : ProcessingVM
{
    private readonly IShoppingCartApi _shoppingCartApi;
    private readonly ICartApi _cartApi;
    private readonly IOrderApi _orderApi;
    private readonly NavigationManager _navigation;
    private readonly SharedStateService _sharedStateService;
    private readonly PaymentApi _paymentApi;
    private readonly AuthUserVM _authUser;

    public CartVM(IShoppingCartApi shoppingCartApi, 
                  ICartApi cartApi,
                  IOrderApi orderApi, 
                  NavigationManager navigation, 
                  SharedStateService sharedStateService, 
                  PaymentApi paymentApi,
                  AuthUserVM authUser)
    {
        _shoppingCartApi = shoppingCartApi;
        _cartApi = cartApi;
        _orderApi = orderApi;
        _navigation = navigation;
        _sharedStateService = sharedStateService;
        _paymentApi = paymentApi;
        _authUser = authUser;
    }

    private IEnumerable<ShoppingCartDTO> _shoppingCarts = new List<ShoppingCartDTO>();
    public IEnumerable<ShoppingCartDTO> ShoppingCarts
    {
        get => _shoppingCarts;
        set => SetProperty(ref _shoppingCarts, value);
    }

    private OrderHeaderDTO _orderHeader = new();
    public OrderHeaderDTO OrderHeader
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
            ShoppingCarts = await _shoppingCartApi.GetAllAsync(OrderHeader.UserId);
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
            var result = await _shoppingCartApi.UpdateAsync(OrderHeader.UserId, productId, updateBy);
            _sharedStateService.TotalCartCount = await _shoppingCartApi.GetTotalCountAsync(OrderHeader.UserId);
            await LoadCartAsync();
        });
    }

    public async Task ProcessOrderCreationAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            await Task.Yield();
            OrderHeader.OrderDetails = await _cartApi.ConvertCartToOrderDetailsAsync(ShoppingCarts.ToList());

            var session = await _paymentApi.CreateCheckoutSessionAsync(OrderHeader);
            OrderHeader.SessionId = session.Id;

            await _orderApi.CreateAsync(OrderHeader);
            _navigation.NavigateTo(session.Url);
        });
    }
}