using Microsoft.AspNetCore.Components;
using Portfolio.ECommerce.Blazor.Data;
using Portfolio.ECommerce.Blazor.Repository.IRepository;
using Portfolio.ECommerce.Blazor.Utility;
using System.Security.Claims;

namespace Portfolio.ECommerce.Blazor.ViewModels
{
    public class OrderListVM : ProcessingVM
    {
        private readonly IOrderRepository _orderRepository;
        private readonly NavigationManager _navigation;
        private readonly AuthUserVM _authUser;

        public OrderListVM(IOrderRepository orderRepository, 
                           NavigationManager navigation,
                           AuthUserVM authUser)
        {
            _orderRepository = orderRepository;
            _navigation = navigation;
            _authUser = authUser;
        }

        private IEnumerable<OrderHeader> _orderHeaders = new List<OrderHeader>();
        public IEnumerable<OrderHeader> OrderHeaders
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

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task AfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadOrderHeadersAsync();
            }
        }

        public async Task LoadOrderHeadersAsync()
        {
            await RunCommandAsync(() => IsProcessing, async () =>
            {
                CheckAuthorization();

                if (IsAdmin)
                    OrderHeaders = await _orderRepository.GetAllAsync();
                else
                    OrderHeaders = await _orderRepository.GetAllAsync(UserId);
            });
        }

        private void CheckAuthorization()
        {
            var user = _authUser.User;

            IsAdmin = user?.IsInRole(SD.Role_Admin) == true;
            UserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public void NavigateToDetails(int id)
        {
            _navigation.NavigateTo($"order/details/{id}");
        }
    }
}