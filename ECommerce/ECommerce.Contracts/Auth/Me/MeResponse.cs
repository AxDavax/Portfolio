using ECommerce.Contracts.Interfaces;

namespace ECommerce.Contracts.Auth.Me;

public class MeResponse : IUserAuth
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
}