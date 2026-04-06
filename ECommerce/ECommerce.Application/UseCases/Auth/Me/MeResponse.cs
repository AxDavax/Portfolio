namespace ECommerce.Application.UseCases.Auth.Me;

public class MeResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
}