using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.ClientPortal.Services.Extensions;
using ECommerce.ClientPortal.Services.State;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.DTO;
using Microsoft.JSInterop;

namespace ECommerce.ClientPortal.ViewModels.Orders;

public class OrderConfirmationVM : ProcessingVM
{
    private readonly IPaymentApi _paymentApi;
    private readonly IJSRuntime _js;
    private readonly SharedStateService _sharedStateService;

    public OrderConfirmationVM(IPaymentApi paymentApi, IJSRuntime js)
    {
        _paymentApi = paymentApi;
        _js = js;   
    }

    private string _sessionId = string.Empty;
    public string SessionId
    {
        get => _sessionId;
        set => SetProperty(ref _sessionId, value);
    }

    private OrderHeaderDTO _orderHeader = new();
    public OrderHeaderDTO OrderHeader
    {
        get => _orderHeader;
        set => SetProperty(ref _orderHeader, value);
    }

    public async Task LoadOrderAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            var response = await _paymentApi.VerifyPaymentAsync(SessionId);
            if (response == null || !response.Success)
            {
                var message = response?.Message ?? "Unknown error";
                await _js.ToastrError($"{message} Please contact support.");
                return;
            }

            OrderHeader = response.Order!;
            _sharedStateService.TotalCartCount = 0;
            await _js.ToastrSuccess($"{response!.Message} Thank you for your order!");
        });
    }
}