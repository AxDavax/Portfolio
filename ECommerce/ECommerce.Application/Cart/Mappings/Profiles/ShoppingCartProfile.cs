using AutoMapper;
using ECommerce.Contracts.DTO;
using ECommerce.Domain.Cart.Models;

namespace ECommerce.Application.Cart.Mappings.Profiles;

public class ShoppingCartProfile : Profile
{
    public ShoppingCartProfile() 
    {
        CreateMap<ShoppingCart, ShoppingCartDTO>() 
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

        CreateMap<ShoppingCartDTO, ShoppingCart>();
    }
}