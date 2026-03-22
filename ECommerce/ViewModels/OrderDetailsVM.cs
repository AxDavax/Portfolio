using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Portfolio.ECommerce.Blazor.Data;
using Portfolio.ECommerce.Blazor.Repository.IRepository;
using Portfolio.ECommerce.Blazor.Services.Extensions;
using Portfolio.ECommerce.Blazor.Utility;

namespace Portfolio.ECommerce.Blazor.ViewModels
{
    public class OrderDetailsVM : BaseVM
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IJSRuntime _js;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public OrderDetailsVM(IOrderRepository orderRepository, IJSRuntime js, AuthenticationStateProvider authenticationStateProvider)
        {
            _orderRepository = orderRepository;
            _js = js;
            _authenticationStateProvider = authenticationStateProvider;
        }

        private bool _isProcessing = true;
        public bool IsProcessing
        {
            get => _isProcessing;
            set => SetProperty(ref _isProcessing, value);
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

        private OrderHeader? _orderHeader = null;
        public OrderHeader? OrderHeader
        {
            get => _orderHeader;
            set => SetProperty(ref _orderHeader, value);
        }

        public async Task InitializeAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            IsAdmin = user?.IsInRole(SD.Role_Admin) == true;
        }

        public async Task AfterRenderAsync(bool firstRender)
        {
            if (firstRender) await LoadOrderAsync();
        }

        public async Task LoadOrderAsync()
        {
            await RunCommandAsync(() => IsProcessing, async () =>
            {
                OrderHeader = await _orderRepository.GetAsync(Id);
            });
        }

        public async Task UpdateStatusAsync(string newStatus)
        {
            await RunCommandAsync(() => IsProcessing, async () =>
            {
                await _orderRepository.UpdateStatusAsync(Id, newStatus, "");
                _js?.ToastrSuccess($"Status updated successfully to {newStatus}");

                await LoadOrderAsync();
            });
        }
    }
}