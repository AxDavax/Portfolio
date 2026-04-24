namespace ECommerce.Contracts.Interfaces;

public interface IUserAuth
{
    Guid UserId { get; set; }
    string Email { get; set; }
    IEnumerable<string> Roles { get; set; }
}