namespace ECommerce.Application.OAuth.Records;

public record ExternalLoginResult(
    bool Success,
    string? Email,
    string? ProviderUserId,
    string? JwtToken,
    string? RefreshToken,
    string? ErrorMessage);