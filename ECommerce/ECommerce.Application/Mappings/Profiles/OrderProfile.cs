using AutoMapper;
using ECommerce.Contracts.DTO;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Mappings.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderHeader, OrderHeaderDTO>().ReverseMap();
        CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
    }
}