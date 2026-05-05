using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.ClientPortal.Utility;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.DTO;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.ViewModels.Orders;

public class OrderListVM : ProcessingVM
{
    private readonly IOrderApi _orderApi;
    private readonly NavigationManager _navigation;
    private readonly AuthUserVM _authUser;

    public OrderListVM(IOrderApi orderApi,
                       NavigationManager navigation,
                       AuthUserVM authUser)
    {
        _orderApi = orderApi;
        _navigation = navigation;
        _authUser = authUser;
    }

    private IEnumerable<OrderHeaderDTO> _orderHeaders = new List<OrderHeaderDTO>();
    public IEnumerable<OrderHeaderDTO> OrderHeaders
    {
        get => _orderHeaders;
        set => SetProperty(ref _orderHeaders, value);
    }

    private bool _isAdmin;
    public bool IsAdmin
    {
        get => _isAdmin;
        set => SetProperty(ref _isAdmin, value);
    }

    private string? _userId;
    public string? UserId
    {
        get => _userId;
        set => SetProperty(ref _userId, value);
    }

    public async Task LoadOrderHeadersAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            await CheckAuthorizationAsync();

            if (IsAdmin)
                OrderHeaders = await _orderApi.GetAllAsync();
            else
                OrderHeaders = await _orderApi.GetAllAsync(UserId);

            OrderHeaders = OrderHeaders.OrderByDescending(o => o.OrderDate).ToList();
        });
    }

    private async Task CheckAuthorizationAsync()
    {
        await _authUser.WaitUntilReadyAsync();

        var user = _authUser.User;
        if (user is null) return;

        if (user?.Identity?.IsAuthenticated != true)
            return;

        IsAdmin = user?.IsInRole(SD.Role_Admin) == true;
        UserId = user?.FindFirst("uid")?.Value;
    }

    public void NavigateToDetails(int id)
    {
        _navigation.NavigateTo($"order/details/{id}");
    }
}