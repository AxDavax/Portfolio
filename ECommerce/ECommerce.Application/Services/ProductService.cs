using ECommerce.Contracts.DTO;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.Models;
using AutoMapper;

namespace ECommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public ProductService(
            IProductRepository repo, 
            ICategoryRepository categoryRepo,
            IMapper mapper, 
            IFileService fileService)
        {
            _repo = repo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ProductDTO> CreateAsync(ProductDTO dto)
        {
            var category = await _categoryRepo.GetByIdAsync(dto.CategoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found");

            var product = _mapper.Map<Product>(dto);
            var created = await _repo.CreateAsync(product);

            var result = _mapper.Map<ProductDTO>(created);
            result.CategoryName = category.Name;

            return result;
        }

        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var products = await _repo.GetAllAsync();
            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);

            foreach (var productDTO in productDTOs)
                productDTO.ImageUrl = _fileService.GetProductImageUrl(productDTO.ImageUrl!);

            return productDTOs;
        }

        public async Task<ProductDTO?> GetByIdAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return null;
            
            var dto = _mapper.Map<ProductDTO?>(product);
            dto!.ImageUrl = _fileService.GetProductImageUrl(dto.ImageUrl!);
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, ProductDTO dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            _mapper.Map(dto, existing);   

            await _repo.UpdateAsync(existing);
            return true;
        }
    }
}