using JarvisAI.Application.Interfaces;
using JarvisAI.Domain.Interfaces;

namespace JarvisAI.Infraestructure.Services;

public class ToolService : IToolService
{
    private readonly IEnumerable<ITool> _tools;

    public ToolService(IEnumerable<ITool> tools)
    {
        _tools = tools;
    }

    public async Task<string> ExecuteToolAsync(string toolName, string input)
    {
        var tool = _tools.FirstOrDefault(t => t.Name == toolName);

        if (tool is null)
            return $"Ferramenta '{toolName}' não encontrada.";

        return await tool.ExecuteAsync(input);
    }

    public IEnumerable<string> GetAvailableTools()
    {
        return _tools.Select(t => $"{t.Name}: {t.Description}");
    }
}