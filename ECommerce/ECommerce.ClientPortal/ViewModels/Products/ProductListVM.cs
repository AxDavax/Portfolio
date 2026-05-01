using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.ClientPortal.Services.Extensions;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.DTO;
using Microsoft.JSInterop;

namespace ECommerce.ClientPortal.ViewModels.Products;

public class ProductListVM : ProcessingVM
{
    private readonly IProductApi _productApi;
    private readonly IJSRuntime _js;

    public ProductListVM(IProductApi productApi, IJSRuntime js)
    {
        _productApi = productApi;
        _js = js;
    }

    private IEnumerable<ProductDTO> _products = new List<ProductDTO>();
    public IEnumerable<ProductDTO> Products
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

    private async Task LoadProductsAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () => 
        {
            Products = await _productApi.GetAllAsync();
        });
    }

    public void HandleDelete(int id)
    {
        DeleteProductID = id;
        _ = _js.InvokeVoidAsync("ShowConfirmationModal");
    }

    public async Task ConfirmDeleteAsync(bool isConfirmed)
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            await _js.InvokeVoidAsync("HideConfirmationModal");

            if (isConfirmed && DeleteProductID != 0)
            {
                var result = await _productApi.DeleteAsync(DeleteProductID);

                if (result) { 
                    _js?.ToastrSuccess("Product deleted successfully");
                
                    var list = Products.ToList();
                    var item = list.FirstOrDefault(p => p.Id == DeleteProductID);
                    if (item != null)
                        list.Remove(item);

                    Products = list;
                }
                else
                    _js?.ToastrError("Error Encountered while deleting");
            }

            DeleteProductID = 0;
        });
    }
}