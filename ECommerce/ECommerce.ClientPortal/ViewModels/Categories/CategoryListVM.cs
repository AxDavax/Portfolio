using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.ClientPortal.Services.Extensions;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.Contracts.DTO;
using Microsoft.JSInterop;

namespace ECommerce.ClientPortal.ViewModels.Categories;

public class CategoryListVM : ProcessingVM
{
    private readonly ICategoryApi _categoryApi;
    private readonly IJSRuntime _js;

    public CategoryListVM(ICategoryApi categoryApi, IJSRuntime js)
    {
        _categoryApi = categoryApi;
        _js = js;
    }

    private IEnumerable<CategoryDTO> _categories = new List<CategoryDTO>();
    public IEnumerable<CategoryDTO> Categories
    {
        get => _categories;
        set => SetProperty(ref _categories, value);
    }

    private int _deleteCategoryID;
    public int DeleteCategoryID
    {
        get => _deleteCategoryID;
        set => SetProperty(ref _deleteCategoryID, value);
    }

    public async Task AfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadCategoriesAsync();
        }
    }

    public async Task LoadCategoriesAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            Categories = await _categoryApi.GetAllAsync();
        });
    }

    public void HandleDelete(int id)
    {
        DeleteCategoryID = id;
        _js.InvokeVoidAsync("ShowConfirmationModal");
    }

    public async Task ConfirmDeleteAsync(bool isConfirmed)
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            await _js.InvokeVoidAsync("HideConfirmationModal");

            if (isConfirmed && DeleteCategoryID != 0)
            {
                var result = await _categoryApi.DeleteAsync(DeleteCategoryID);

                if (result)
                    _js?.ToastrSuccess("Category deleted successfully");
                else
                    _js?.ToastrError("Error deleting category");

                Categories = Categories.Where(c => c.Id != DeleteCategoryID).ToList();
            }

            DeleteCategoryID = 0;
        });
        
        OnStateChanged();
    }
}