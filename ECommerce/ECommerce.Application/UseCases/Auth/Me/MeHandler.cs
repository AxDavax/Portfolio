using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.UseCases.Auth.Me;

public class MeHandler
{
    private readonly IUserRepository _users;

    public MeHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task<MeResponse> Handle(MeRequest request)
    {
        // 1. Loads the user
        var user = await _users.GetByIdAsync(request.UserId);
        if (user == null)
            throw new UnauthorizedAccessException("User not found");

        // 2. Loads the roles
        var roles = await _users.GetRolesAsync(user.Id);

        // 3. Returns the response
        return new MeResponse
        {
            UserId = user.Id,
            Email = user.Email,
            Roles = roles
        };
    }
}