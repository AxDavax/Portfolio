using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.ClientPortal.Services.Extensions;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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

    private string? _directoryPath = string.Empty;
    public string DirectoryPath
    {
        get => _directoryPath!;
        set => SetProperty(ref _directoryPath, value);
    }

    public async Task AfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadProductAndCategoryListAsync();
            StateChanged?.Invoke();
        }
    }

    public async Task LoadProductAndCategoryListAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            if (Id > 0)
            {
                Product = await _productApi.GetByIdAsync(Id) ?? new ProductDTO();
            }   
            
            Categories = await _categoryApi.GetAllAsync();
        });
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

            _navigation.NavigateTo("product");
        });
    }

    public async Task LoadFilesAsync(InputFileChangeEventArgs e)
    {
        var file = e.File;
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}";
            
        await using var fileStream = file.OpenReadStream(5_000_000);

        var uploadedFileName = await _fileApi.UploadProductImageAsync(fileStream, fileName);

        if (uploadedFileName != null)
        {
            Product.ImageUrl = uploadedFileName;
            await _js.ToastrSuccess("Image Uploaded Successfully");
        }
        else 
            await _js.ToastrError("Image Upload Failed");
    }

    public async Task DeleteImageAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            if (string.IsNullOrWhiteSpace(Product.ImageUrl)) return;

            var fileToDelete = Product.ImageUrl.Split('/').Last();

            var result = await _fileApi.DeleteProductImageAsync(fileToDelete);

            if (result)
            {
                Product.ImageUrl = null;
                await _js.ToastrSuccess("Image deleted successfully");
            }
            else
                await _js.ToastrError("Error deleting image");
        });
    }
}