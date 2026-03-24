using Microsoft.JSInterop;
using Portfolio.ECommerce.Blazor.Data;
using Portfolio.ECommerce.Blazor.Repository.IRepository;
using Portfolio.ECommerce.Blazor.Services.Extensions;
using Portfolio.ECommerce.Blazor.ViewModels.Core;

namespace Portfolio.ECommerce.Blazor.ViewModels.Categories
{
    public class CategoryListVM : ProcessingVM
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IJSRuntime _js;

        public CategoryListVM(ICategoryRepository categoryRepository, IJSRuntime js)
        {
            _categoryRepository = categoryRepository;
            _js = js;
        }

        private IEnumerable<Category> _categories = new List<Category>();
        public IEnumerable<Category> Categories
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

        public async Task InitializeAsync()
        {
            await Task.Delay(5000);
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
                Categories = await _categoryRepository.GetAllAsync();
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
                    var result = await _categoryRepository.DeleteAsync(DeleteCategoryID);

                    if (result)
                        _js?.ToastrSuccess("Category deleted successfully");
                    else
                        _js?.ToastrError("Error deleting category");

                    await LoadCategoriesAsync();
                }

                DeleteCategoryID = 0;
            });
        }
    }
}