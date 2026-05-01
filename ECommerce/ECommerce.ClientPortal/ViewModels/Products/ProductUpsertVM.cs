using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.ClientPortal.Services.Extensions;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ECommerce.ClientPortal.ViewModels.Products;

public class ProductUpsertVM : ProcessingVM
{
    private readonly ICategoryApi _categoryApi;
    private readonly IProductApi _productApi;
    private readonly NavigationManager _navigation;
    private readonly IJSRuntime _js;
    private readonly IFileApi _fileApi;

    public ProductUpsertVM(ICategoryApi categoryApi, IProductApi productApi, 
                           NavigationManager navigation, IJSRuntime js, 
                           IFileApi fileApi)
    {
        _categoryApi = categoryApi;
        _productApi = productApi;
        _navigation = navigation;
        _js = js;
        _fileApi = fileApi;
    }

    private int _id;
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    private bool IsUpdate => Id > 0;

    private ProductDTO _product = new();
    public ProductDTO Product
    {
        get => _product;
        set => SetProperty(ref _product, value);
    }

    private IEnumerable<CategoryDTO> _categories = new List<CategoryDTO>();
    public IEnumerable<CategoryDTO> Categories
    {
        get => _categories;
        set => SetProperty(ref _categories, value);
    }

    public string GetPreviewUrl()
    {
        if (!IsUpdate)
        {
            if (string.IsNullOrWhiteSpace(Product.ImageUrl))
                return string.Empty;

            return _fileApi.GetProductImageUrl(Product.ImageUrl);
        }
        
        return Product.ImageUrl ?? string.Empty;
    }

    public async Task UpsertProductAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            if (Product.Id == 0)
            {
                await _productApi.CreateAsync(Product);
                await _js.ToastrSuccess("Product Created Successfully");
            }
            else
            {
                await _productApi.UpdateAsync(Product.Id, Product);
                await _js.ToastrSuccess("Product Updated Successfully");
            }
        
        });
        
        _navigation.NavigateTo("product");
    }
}