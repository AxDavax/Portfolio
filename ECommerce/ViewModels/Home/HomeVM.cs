using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Portfolio.ECommerce.Blazor.Data;
using Portfolio.ECommerce.Blazor.Repository.IRepository;
using Portfolio.ECommerce.Blazor.Services;
using Portfolio.ECommerce.Blazor.Services.Extensions;
using Portfolio.ECommerce.Blazor.ViewModels.Core;

namespace Portfolio.ECommerce.Blazor.ViewModels.Home
{
    public class HomeVM : ProcessingVM
    {
        private readonly IShoppingCartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly NavigationManager _navigation;
        private readonly SharedStateService _sharedStateService;
        private readonly IJSRuntime _js;
        private readonly AuthUserVM _authUser;

        public HomeVM(IShoppingCartRepository cartRepository, 
                      IProductRepository productRepository, 
                      ICategoryRepository categoryRepository, 
                      NavigationManager navigation, 
                      SharedStateService sharedStateService, 
                      IJSRuntime js, 
                      AuthUserVM authUser)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _navigation = navigation;
            _sharedStateService = sharedStateService;
            _js = js;
            _authUser = authUser;
        }

        private IEnumerable<Product> _products = new List<Product>();
        public IEnumerable<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private IEnumerable<Category> _categories = new List<Category>();
        public IEnumerable<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        private IEnumerable<Product> _filteredProducts = new List<Product>();
        public IEnumerable<Product> FilteredProducts
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
                Categories = await _categoryRepository.GetAllAsync();
                Products = await _productRepository.GetAllAsync();
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

        public async Task AddOrderItem(Product product)
        {
            await RunCommandAsync(() => IsProcessing, async () =>
            {
                var user = _authUser.User;
                if (user is null) return;

                var authenticated = user.Identity is not null && user.Identity.IsAuthenticated;

                if (!authenticated)
                {
                    _navigation.NavigateTo("/Account/Login", forceLoad: true);
                    return;
                }
                else
                {
                    var userId = user.FindFirst(u => u.Type.Contains("nameidentifier"))?.Value;
                  
                    var result = await
                    _cartRepository.UpdateCartAsync(userId, product.Id, 1);
                    _sharedStateService.TotalCartCount = await _cartRepository.GetTotalCartCountAsync(userId);

                    if (result)
                    {
                        _js?.ToastrSuccess("Product added to cart successfully");
                    }
                    else
                    {
                        _js?.ToastrError("Error encountered");
                    }
                }
            });
        }
    }
}