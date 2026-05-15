using AutoMapper;
using ECommerce.Application.Auth.Models;
using ECommerce.Domain.Auth.Models;

namespace ECommerce.Application.Mappings.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<SqlUser, User>();

        CreateMap<User, SqlUser>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
}