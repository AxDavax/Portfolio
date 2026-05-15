using AutoMapper;
using ECommerce.Contracts.DTO;
using ECommerce.Domain.Orders.Models;

namespace ECommerce.Application.Orders.Mappings.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderHeader, OrderHeaderDTO>().ReverseMap();
        CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
    }
}