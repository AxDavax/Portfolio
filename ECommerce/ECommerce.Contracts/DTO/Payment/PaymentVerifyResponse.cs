namespace ECommerce.Contracts.DTO.Payment;

public class PaymentVerifyResponse
{
    public bool Success { get; set; }
    public OrderHeaderDTO? Order { get; set; }
    public string Message { get; set; } = string.Empty;
}