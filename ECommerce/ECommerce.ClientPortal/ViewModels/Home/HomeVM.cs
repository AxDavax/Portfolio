using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ECommerce.ClientPortal.Services.Extensions;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.DTO;
using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.ClientPortal.Services.State;

namespace ECommerce.ClientPortal.ViewModels.Home
{
    public class HomeVM : ProcessingVM
    {
        private readonly IShoppingCartApi _cartApi;
        private readonly IProductApi _productApi;
        private readonly ICategoryApi _categoryApi;
        private readonly NavigationManager _navigation;
        private readonly SharedStateService _sharedStateService;
        private readonly IJSRuntime _js;
        private readonly AuthUserVM _authUser;

        public HomeVM(IShoppingCartApi cartApi, 
                      IProductApi productApi, 
                      ICategoryApi categoryApi, 
                      NavigationManager navigation, 
                      SharedStateService sharedStateService, 
                      IJSRuntime js, 
                      AuthUserVM authUser)
        {
            _cartApi = cartApi;
            _productApi = productApi;
            _categoryApi = categoryApi;
            _navigation = navigation;
            _sharedStateService = sharedStateService;
            _js = js;
            _authUser = authUser;
        }

        private IEnumerable<ProductDTO> _products = new List<ProductDTO>();
        public IEnumerable<ProductDTO> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private IEnumerable<CategoryDTO> _categories = new List<CategoryDTO>();
        public IEnumerable<CategoryDTO> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        private IEnumerable<ProductDTO> _filteredProducts = new List<ProductDTO>();
        public IEnumerable<ProductDTO> FilteredProducts
        {
            get => _filteredProducts;
            set => SetProperty(ref _filteredProducts, value);
        }

        private int _selectedCategoryId;
        public int SelectedCategoryId
        {
            get => _selectedCategoryId;
            set => SetProperty(ref _selectedCategoryId, value);
        }

        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task AfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadProductsAsync();
            } 
        }

        public async Task LoadProductsAsync()
        {
            await RunCommandAsync(() => IsProcessing, async () =>
            {
                Categories = await _categoryApi.GetAllAsync();
                Products = await _productApi.GetAllAsync();
                FilterProducts(0);
            });
        }

        public void FilterProducts(int categoryId)
        {
            if (categoryId == 0)
            {
                FilteredProducts = Products;
                SelectedCategoryId = categoryId;
                return;
            }
            else
            {
                FilteredProducts = Products.Where(u => u.CategoryId == categoryId).ToList();
                SelectedCategoryId = categoryId;
                SearchText = string.Empty;
            }
        }

        public void FilterProductByName(string newValueOfSearchText)
        {
            if (string.IsNullOrWhiteSpace(newValueOfSearchText))
            {
                FilteredProducts = Products;
            }
            else
            {
                FilteredProducts = Products
                .Where(u => u.Name.Contains(newValueOfSearchText, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            SearchText = newValueOfSearchText;
        }

        public string GetActiveTabClass(int categoryId) 
            => categoryId == _selectedCategoryId ? "active" : string.Empty;

        public async Task AddOrderItem(ProductDTO product)
        {
            await RunCommandAsync(() => IsProcessing, async () =>
            {
                Console.WriteLine("IsReady = " + _authUser.IsReady);
                while (!_authUser.IsReady)
                    await Task.Delay(50);

                var user = _authUser.User;
                if (user is null) return;

                var authenticated = user?.Identity?.IsAuthenticated ?? false;
                if (!authenticated)
                {
                    _navigation.NavigateTo("/Auth/Login", forceLoad: true);
                    return;
                }
                
                var userId = user.FindFirst(u => u.Type.Contains("nameidentifier"))?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _navigation.NavigateTo("/Auth/Login", forceLoad: true);
                    return;
                }

                try
                {
                    var result = await _cartApi.UpdateAsync(userId!, product.Id, 1);
                    _sharedStateService.TotalCartCount = await _cartApi.GetTotalCountAsync(userId!);

                    if (result)
                        _js?.ToastrSuccess("Product added to cart successfully");
                    else
                        _js?.ToastrError("Error encountered");
                }
                catch
                {
                    _navigation.NavigateTo("/Auth/Login", forceLoad: true);
                }
            });
        }
    }
}