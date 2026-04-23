using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly string _productStoragePath;

        public FileController(IFileService fileService, IWebHostEnvironment env)
        {
            _fileService = fileService;
            _productStoragePath = Path.Combine(env.ContentRootPath, "storage", "products");
        }

        // GET: api/files/products/{fileName}
        [HttpGet("products/{fileName}")]
        public IActionResult GetProductImage(string fileName)
        {
            var filePath = Path.Combine(_productStoragePath, fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var mime = "image/" + Path.GetExtension(fileName).TrimStart('.');
            return PhysicalFile(filePath, mime);
        }

        // POST: api/files/products
        [HttpPost("products")]
        public async Task<IActionResult> UploadProductImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();

            var savedFilePath = await _fileService.UploadProductImageAsync(
                stream,
                file.FileName,
                file.ContentType
            );

            var fileName = Path.GetFileName(savedFilePath);
            var url = _fileService.GetProductImageUrl(fileName);

            return Ok(new { url });
        }

        // DELETE: api/files/products/{fileName}
        [HttpDelete("products/{fileName}")]
        public async Task<IActionResult> DeleteProductImage(string fileName)
        {
            var success = await _fileService.DeleteProductImageAsync(fileName);

            if (!success)
                return NotFound();

            return Ok();
        }
    }
}