namespace ECommerce.Blazor_WebApp.Client.Services.API.Interfaces;

public interface IFileApi
{
    Task<string?> UploadProductImageAsync(Stream fileStream, string fileName);
    Task<bool> DeleteProductImageAsync(string fileName);
}