namespace ECommerce.Contracts.DTO.Payment;

public class CheckoutSessionResponse
{
    public string Id { get; set; } = default!;
    public string Url { get; set; } = default!;
}