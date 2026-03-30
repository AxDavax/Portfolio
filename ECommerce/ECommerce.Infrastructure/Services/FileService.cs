using ECommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly string _productStoragePath;
    private readonly string _productBaseUrl;

    public FileService(IConfiguration config, IWebHostEnvironment env)
    {
        _productStoragePath = Path.Combine(env.ContentRootPath, "storage", "products");
        
        _productBaseUrl = config["FileStorage:ProductBaseUrl"] ?? "/api/files/products/";

        if(!Directory.Exists(_productStoragePath))
            Directory.CreateDirectory(_productStoragePath);
    }

    public async Task<bool> DeleteProductImageAsync(string fileName)
    {
        var filePath = Path.Combine(_productStoragePath, fileName);

        if (!File.Exists(filePath)) return false;

        await Task.Run(() => File.Delete(filePath));
        return true;
    }

    public string GetProductImageUrl(string fileName) => $"{_productBaseUrl}{fileName}";

    public async Task<string> UploadProductImageAsync(Stream fileStream, string fileName, string contentType)
    {
        var extension = Path.GetExtension(fileName);
        var newFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(_productStoragePath, newFileName);

        using (var output = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(output);
        }

        return filePath;
    }
}