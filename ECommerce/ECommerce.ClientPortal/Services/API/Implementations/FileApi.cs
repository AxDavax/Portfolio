using System.Net.Http.Headers;
using System.Text.Json;
using ECommerce.ClientPortal.Models;
using ECommerce.ClientPortal.Services.API.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ECommerce.ClientPortal.Services.API.Implementations;

public class FileApi : BaseApi, IFileApi
{
    private readonly IConfiguration _config;

    public FileApi(HttpClient http, NavigationManager nav, IConfiguration config) : base(http, nav) 
    {
        _config = config;
    }

    public Task<bool> DeleteProductImageAsync(string fileName)
        => SafeDelete($"api/files/products/{fileName}");

    public string GetProductImageUrl(string fileName)
        => $"{_config["Api:BaseUrl"]}/products/{fileName}";

    public async Task<string?> UploadProductImageAsync(Stream fileStream, string fileName)
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(fileStream);

        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        content.Add(fileContent, "file", fileName);

        var json = await SafePostMultipart("api/files/products", content);
        var result = JsonSerializer.Deserialize<UploadResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result?.FileName;
    }
}