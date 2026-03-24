using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Portfolio.ECommerce.Blazor.Data;
using Portfolio.ECommerce.Blazor.Repository.IRepository;
using Portfolio.ECommerce.Blazor.Services.Extensions;

namespace Portfolio.ECommerce.Blazor.ViewModels
{
    public class ProductUpsertVM : ProcessingVM
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly NavigationManager _navigation;
        private readonly IJSRuntime _js;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductUpsertVM(ICategoryRepository categoryRepository, 
                               IProductRepository productRepository, 
                               NavigationManager navigation, IJSRuntime js, 
                               IWebHostEnvironment webHostEnvironment)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _navigation = navigation;
            _js = js;
            _webHostEnvironment = webHostEnvironment;
        }

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private Product _product = new();
        public Product Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        private IEnumerable<Category> _categories = new List<Category>();
        public IEnumerable<Category> Categories
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

        public Task InitializeAsync()
        {
            DirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "product");
            return Task.CompletedTask;
        }

        public async Task AfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadProductAndCategoryListAsync();   
            }
        }

        private async Task LoadProductAndCategoryListAsync()
        {
            await RunCommandAsync(() => IsProcessing, async () =>
            {
                if (Id > 0) Product = await _productRepository.GetAsync(Id);
                Categories = await _categoryRepository.GetAllAsync();
            });
        }

        public async Task UpsertProductAsync()
        {
            await RunCommandAsync(() => IsProcessing, async () =>
            {
                if (Product.Id == 0)
                {
                    await _productRepository.CreateAsync(Product);
                    await _js.ToastrSuccess("Product Created Successfully");
                }
                else
                {
                    await _productRepository.UpdateAsync(Product);
                    await _js.ToastrSuccess("Product Updated Successfully");
                }

                _navigation.NavigateTo("product");
            });
        }

        public async Task LoadFilesAsync(InputFileChangeEventArgs e)
        {
            await RunCommandAsync(() => IsProcessing, async () =>
            {
                var file = e.File;
                System.IO.FileInfo fileInfo = new(file.Name);
                var newFileName = $"{Guid.NewGuid()}.{fileInfo.Extension}";
                if (!Directory.Exists(DirectoryPath))
                    Directory.CreateDirectory(DirectoryPath);

                var path = Path.Combine(DirectoryPath, newFileName);

                await using FileStream fileStream = new(path, FileMode.Create);
                await file.OpenReadStream(file.Size).CopyToAsync(fileStream);
                Product.ImageUrl = $"/images/product/{newFileName}";
            });
        }

        public void DeleteImage()
        {
            if (Product.ImageUrl == null) return;

            var fileToDelete = Product.ImageUrl.Split('/').Reverse().First();

            var filePathToDeleteImage = Path.Combine(DirectoryPath!, fileToDelete);

            if (!File.Exists(filePathToDeleteImage))
            {
                Product.ImageUrl = null;
                return;
            }

            File.Delete(filePathToDeleteImage);
            Product.ImageUrl = null;
            return;
        }
    }
}