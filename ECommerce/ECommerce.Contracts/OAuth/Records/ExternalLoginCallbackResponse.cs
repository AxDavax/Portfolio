namespace ECommerce.Contracts.OAuth.Records;

public record ExternalLoginCallbackResponse(
    bool Success,
    string? Email,
    string? ProviderUserId,
    string? ErrorMessage);