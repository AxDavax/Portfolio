using System.Net.Http.Headers;
using ECommerce.Blazor_WebApp.Client.Services.API.Interfaces;

namespace ECommerce.Blazor_WebApp.Client.Services.API.Implementations
{
    public class FileApi : IFileApi
    {
        private readonly HttpClient _http;

        public FileApi(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> DeleteProductImageAsync(string fileName)
        {
            var response = await _http.DeleteAsync($"api/files/products/{fileName}");
            return response.IsSuccessStatusCode;
        }

        public async Task<string?> UploadProductImageAsync(Stream fileStream, string fileName)
        {
            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(fileStream);

            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            content.Add(fileContent, "file", fileName);

            var response = await _http.PostAsync("api/files/products", content);

            if(!response.IsSuccessStatusCode) 
                return null;

            return await response.Content.ReadAsStringAsync();
        }
    }
}