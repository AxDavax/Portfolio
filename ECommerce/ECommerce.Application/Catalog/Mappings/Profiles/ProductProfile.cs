using AutoMapper;
using ECommerce.Contracts.DTO;
using ECommerce.Domain.Catalog.Models;

namespace ECommerce.Application.Catalog.Mappings.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<ProductDTO, Product>();
    }
}