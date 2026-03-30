namespace ECommerce.Domain.Interfaces;

public interface IFileService
{
    Task<string> UploadProductImageAsync(Stream fileStream, string fileName, string contentType);
    Task<bool> DeleteProductImageAsync(string fileName);
    string GetProductImageUrl(string fileName);
}