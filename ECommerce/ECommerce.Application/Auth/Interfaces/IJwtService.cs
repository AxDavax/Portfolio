using ECommerce.Application.Models;
using ECommerce.Domain.Auth.Models;

namespace ECommerce.Application.Auth.Interfaces;

public interface IJwtService
{
    JwtResult GenerateToken(User user, IEnumerable<string> roles);
}