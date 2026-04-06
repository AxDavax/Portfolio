namespace ECommerce.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(Guid userId, string email, IEnumerable<string> roles);
}