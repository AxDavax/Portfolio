using ECommerce.Application.Interfaces;
using RazorLight;

namespace ECommerce.Infrastructure.Services;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly RazorLightEngine _engine;
    private readonly string _templateRoot;

    public EmailTemplateService()
    {
        _templateRoot = Path.Combine(AppContext.BaseDirectory, "EmailTemplates");

        _engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(_templateRoot)
            .UseMemoryCachingProvider()
            .Build();
    }

    public async Task<string> RenderAsync<T>(string templateName, T model)
    {
        var filePath = Path.Combine(_templateRoot, $"{templateName}.cshtml");

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Email template '{templateName}' not found.", filePath);

        return await _engine.CompileRenderAsync($"{templateName}.cshtml", model);
    }
}