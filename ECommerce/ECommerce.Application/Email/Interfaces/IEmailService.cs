namespace ECommerce.Application.Email.Interfaces;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string htmlBody);
}