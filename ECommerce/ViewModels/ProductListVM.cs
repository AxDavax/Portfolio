using Microsoft.JSInterop;
using Portfolio.ECommerce.Blazor.Data;
using Portfolio.ECommerce.Blazor.Repository.IRepository;
using Portfolio.ECommerce.Blazor.Services.Extensions;

namespace Portfolio.ECommerce.Blazor.ViewModels
{
    public class ProductListVM : ProcessingVM
    {
        private readonly IProductRepository _productRepository;
        private readonly IJSRuntime _js;

        public ProductListVM(IProductRepository productRepository, IJSRuntime js)
        {
            _productRepository = productRepository;
            _js = js;
        }

        private IEnumerable<Product> _products = new List<Product>();
        public IEnumerable<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private int _deleteProductID;
        public int DeleteProductID
        {
            get => _deleteProductID;
            set => SetProperty(ref _deleteProductID, value);
        }

        public async Task InitializeAsync()
        {
            
        }

        public async Task AfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadProductsAsync();
            }
        }

        private async Task LoadProductsAsync()
        {
            await RunCommandAsync(() => IsProcessing, async () => 
            {
                Products = await _productRepository.GetAllAsync();
            });
        }

        public void HandleDelete(int id)
        {
            DeleteProductID = id;
            _js.InvokeVoidAsync("ShowConfirmationModal");
        }

        public async Task ConfirmDeleteAsync(bool isConfirmed)
        {
            await RunCommandAsync(() => IsProcessing, async () =>
            {
                await _js.InvokeVoidAsync("HideConfirmationModal");

                if (isConfirmed && DeleteProductID != 0)
                {
                    var result = await _productRepository.DeleteAsync(DeleteProductID);

                    if (result)
                        _js?.ToastrSuccess("Product deleted successfully");
                    else
                        _js?.ToastrError("Error Encountered while deleting");

                    await LoadProductsAsync();
                }

                DeleteProductID = 0;
            });
        }
    }
}