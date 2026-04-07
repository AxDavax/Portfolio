using AutoMapper;
using ECommerce.Contracts.DTO;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Mappings.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDTO>().ReverseMap();
    }
}