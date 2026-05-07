using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.ClientPortal.Services.Extensions;
using ECommerce.ClientPortal.Utility;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.DTO;
using Microsoft.JSInterop;

namespace ECommerce.ClientPortal.ViewModels.Orders;

public class OrderDetailsVM : ProcessingVM
{
    private readonly IOrderApi _orderApi;
    private readonly IJSRuntime _js;
    private readonly AuthUserVM _authUser;

    public OrderDetailsVM(IOrderApi orderApi,
                          IJSRuntime js,
                          AuthUserVM authUser)
    {
        _orderApi = orderApi;
        _js = js;
        _authUser = authUser;
    }

    private int _id;
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    private bool _isAdmin;
    public bool IsAdmin
    {
        get => _isAdmin;
        set => SetProperty(ref _isAdmin, value);
    }

    private bool _isLoaded;
    public bool IsLoaded
    {
        get => _isLoaded;
        set => SetProperty(ref _isLoaded, value);
    }

    private bool _isAuthorized;
    public bool IsAuthorized
    {
        get => _isAuthorized;
        set => SetProperty(ref _isAuthorized, value);
    }

    private OrderHeaderDTO? _orderHeader = null;
    public OrderHeaderDTO? OrderHeader
    {
        get => _orderHeader;
        set => SetProperty(ref _orderHeader, value);
    }

    public async Task InitializeAsync()
    {
        await _authUser.WaitUntilReadyAsync();

        var user = _authUser.User;
        if (user is null)
        {
            IsAuthorized = false;
            return;
        }

        IsAuthorized = user.Identity?.IsAuthenticated == true;
        IsAdmin = user.IsInRole(SD.Role_Admin);
    }

    public async Task LoadOrderCoreAsync()
    {
        var header = await _orderApi.GetByIdAsync(Id);

        if (!IsAdmin && header?.UserId.ToString() != _authUser.UserId)
        {
            _js.ToastrError("You are not allowed to view this order.");
            OrderHeader = null;
            return;
        }

        OrderHeader = header;
    }

    public async Task LoadOrderAsync()
    {
        if (!IsAuthorized)
        {
            _js.ToastrError("You are not authorized to view this order.");
            return;
        }

        await RunCommandAsync(() => IsProcessing, async () =>
        {
            IsLoaded = false;
            OrderHeader = null;

            await LoadOrderCoreAsync();

            IsLoaded = true;
        });
    }

    public async Task UpdateStatusAsync(string newStatus)
    {
        if (!IsAdmin)
        {
            _js.ToastrError("Only administrators can update order status.");
            return;
        }

        await RunCommandAsync(() => IsProcessing, async () =>
        {
            await _orderApi.UpdateAsync(Id, newStatus, "");
            _js?.ToastrSuccess($"Status updated successfully to {newStatus}");

            await LoadOrderCoreAsync();
        });
    }
}