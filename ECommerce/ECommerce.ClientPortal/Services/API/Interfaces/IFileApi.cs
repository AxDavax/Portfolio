namespace ECommerce.ClientPortal.Services.API.Interfaces;

public interface IFileApi
{
    Task<string?> UploadProductImageAsync(Stream fileStream, string fileName);
    Task<bool> DeleteProductImageAsync(string fileName);
}