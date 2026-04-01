using ECommerce.Application.DTO;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly ICategoryRepository _categoryRepo;

        public ProductService(IProductRepository repo, ICategoryRepository categoryRepo)
        {
            _repo = repo;
            _categoryRepo = categoryRepo;
        }

        public async Task<ProductDTO> CreateAsync(ProductDTO dto)
        {
            var category = await _categoryRepo.GetByIdAsync(dto.CategoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found");

            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                SpecialTag = dto.SpecialTag,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId
            };

            var created = await _repo.CreateAsync(product);

            dto.Id = created.Id;
            dto.CategoryName = category.Name;

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var products = await _repo.GetAllAsync();

            return products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                SpecialTag = p.SpecialTag,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name!
            });
        }

        public async Task<ProductDTO?> GetByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;

            return new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                SpecialTag = p.SpecialTag,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name!
            };
        }

        public async Task<bool> UpdateAsync(int id, ProductDTO dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return false;

            existing.Name = dto.Name;
            existing.Price = dto.Price;
            existing.Description = dto.Description;
            existing.SpecialTag = dto.SpecialTag;
            existing.ImageUrl = dto.ImageUrl;
            existing.CategoryId = dto.CategoryId;

            await _repo.UpdateAsync(existing);
            return true;
        }
    }
}