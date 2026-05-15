using AutoMapper;
using ECommerce.Contracts.DTO;
using ECommerce.Domain.Catalog.Models;

namespace ECommerce.Application.Catalog.Mappings.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDTO>().ReverseMap();
    }
}