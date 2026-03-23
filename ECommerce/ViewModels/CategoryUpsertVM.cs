using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Portfolio.ECommerce.Blazor.Data;
using Portfolio.ECommerce.Blazor.Repository.IRepository;
using Portfolio.ECommerce.Blazor.Services.Extensions;

namespace Portfolio.ECommerce.Blazor.ViewModels
{
    public class CategoryUpsertVM : ProcessingVM
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IJSRuntime _js;
        private readonly NavigationManager _navigation;

        public CategoryUpsertVM(ICategoryRepository categoryRepository, IJSRuntime js, NavigationManager navigation)
        {
            _categoryRepository = categoryRepository;
            _js = js;
            _navigation = navigation;
        }

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private Category _category = new();
        public Category Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        public async Task InitializeAsync()
        {
        
        }

        public async Task AfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadCategoryAsync();
            }
        }

        public async Task LoadCategoryAsync()
        {
            await RunCommandAsync(() => IsProcessing, async () =>
            {
                if (Id > 0) Category = await _categoryRepository.GetAsync(Id);
            });
        }

        public async Task UpsertCategoryAsync()
        {
            await RunCommandAsync(() => IsProcessing, async () =>
            {
                if (Category.Id == 0)
                {
                    await _categoryRepository.CreateAsync(Category);
                    await _js.ToastrSuccess("Category Created Successfully");
                }
                else
                {
                    await _categoryRepository.UpdateAsync(Category);
                    await _js.ToastrSuccess("Category Updated Successfully");
                }

                _navigation.NavigateTo("category");
            });
        }
    }
}