namespace ECommerce.Application.Interfaces;

public interface IEmailTemplateService
{
    Task<string> RenderAsync<T>(string templateName, T model);
}