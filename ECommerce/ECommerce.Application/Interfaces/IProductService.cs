using ECommerce.Contracts.DTO;

namespace ECommerce.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDTO> CreateAsync(ProductDTO dto);
        Task<bool> UpdateAsync(int id, ProductDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<ProductDTO?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDTO>> GetAllAsync();
    }
}