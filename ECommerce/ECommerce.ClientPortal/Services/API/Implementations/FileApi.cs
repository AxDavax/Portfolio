using System.Net.Http.Headers;
using ECommerce.ClientPortal.Services.API.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.Services.API.Implementations;

public class FileApi : BaseApi, IFileApi
{
    public FileApi(HttpClient http, NavigationManager nav) : base(http, nav) { }

    public Task<bool> DeleteProductImageAsync(string fileName)
        => SafeDelete($"api/files/products/{fileName}");

    public Task<string?> UploadProductImageAsync(Stream fileStream, string fileName)
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(fileStream);

        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        content.Add(fileContent, "file", fileName);

        return SafePost<string>("api/files/products", content);
    }
}