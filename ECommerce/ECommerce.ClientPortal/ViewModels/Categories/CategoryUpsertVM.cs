using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ECommerce.ClientPortal.ViewModels.Core;
using ECommerce.ClientPortal.Services.API.Interfaces;
using ECommerce.Contracts.DTO;
using ECommerce.ClientPortal.Services.Extensions;

namespace ECommerce.ClientPortal.ViewModels.Categories;

public class CategoryUpsertVM : ProcessingVM
{
    private readonly ICategoryApi _categoryApi;
    private readonly IJSRuntime _js;
    private readonly NavigationManager _navigation;

    public CategoryUpsertVM(ICategoryApi categoryApi, IJSRuntime js, NavigationManager navigation)
    {
        _categoryApi = categoryApi;
        _js = js;
        _navigation = navigation;
    }

    private int _id;
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    private CategoryDTO _category = new();
    public CategoryDTO Category
    {
        get => _category;
        set => SetProperty(ref _category, value);
    }

    public async Task LoadCategoryAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            if (Id > 0) Category = await _categoryApi.GetByIdAsync(Id);
        });
    }

    public async Task UpsertCategoryAsync()
    {
        await RunCommandAsync(() => IsProcessing, async () =>
        {
            if (Category.Id == 0)
            {
                await _categoryApi.CreateAsync(Category);
                await _js.ToastrSuccess("Category Created Successfully");

                Category = new CategoryDTO();
            }
            else
            {
                await _categoryApi.UpdateAsync(Id, Category);
                await _js.ToastrSuccess("Category Updated Successfully");
            }
        });
        
        _navigation.NavigateTo("category");
    }
}