using ECommerce.Application.Models;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Interfaces;

public interface IJwtService
{
    JwtResult GenerateToken(User user, IEnumerable<string> roles);
}