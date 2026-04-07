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

        public ProductService(
            IProductRepository repo, 
            ICategoryRepository categoryRepo,
            IMapper mapper)
        {
            _repo = repo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
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
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO?> GetByIdAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            return (product == null) ? null : _mapper.Map<ProductDTO?>(product);
        }

        public async Task<bool> UpdateAsync(int id, ProductDTO dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            _mapper.Map(existing, dto);   

            await _repo.UpdateAsync(existing);
            return true;
        }
    }
}