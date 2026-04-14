using ECommerce.Domain.Models;

namespace ECommerce.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user, IEnumerable<string> roles);
}