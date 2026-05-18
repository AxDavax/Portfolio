using System.Text.Json.Serialization;

namespace ECommerce.Application.OAuth.Models;

public class GoogleUserInfo
{
    [JsonPropertyName("sub")]
    public string Sub { get; set; } = default!;

    [JsonPropertyName("email")]
    public string Email { get; set; } = default!;

    [JsonPropertyName("email_verified")]
    public bool EmailVerified { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("given_name")]
    public string? GivenName { get; set; }

    [JsonPropertyName("family_name")]
    public string? FamilyName { get; set; }

    [JsonPropertyName("picture")]
    public string? Picture { get; set; }

    [JsonPropertyName("locale")]
    public string? Locale { get; set; }
}