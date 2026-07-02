using JarvisAI.Domain.Interfaces;
using System.Text.Json;
using System.Web;

namespace JarvisAI.Infraestructure.Tools;

public class NewsTool : ITool
{
    public string Name => "NewsTool";
    public string Description => "Busca as últimas notícias sobre um tema. Input: tema das notícias ex: tecnologia, brasil, esportes";

    private readonly HttpClient _httpClient;

    public NewsTool()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "JarvisAI/1.0");
    }

    public async Task<string> ExecuteAsync(string input)
    {
        try
        {
            var query = HttpUtility.UrlEncode(input);
            var url = $"https://api.duckduckgo.com/?q={query}+notícias&format=json&no_html=1&skip_disambig=1";

            var response = await _httpClient.GetStringAsync(url);
            var json = JsonSerializer.Deserialize<JsonElement>(response);

            var relatedTopics = json.GetProperty("RelatedTopics");
            var result = $"Notícias sobre '{input}':\n";
            var count = 0;

            foreach (var topic in relatedTopics.EnumerateArray())
            {
                if (count >= 5) break;
                if (topic.TryGetProperty("Text", out var text) && !string.IsNullOrEmpty(text.GetString()))
                {
                    result += $"• {text.GetString()}\n";
                    count++;
                }
            }

            return count == 0
                ? $"Não encontrei notícias sobre '{input}'."
                : result;
        }
        catch
        {
            return $"Não consegui buscar notícias sobre '{input}'.";
        }
    }
}