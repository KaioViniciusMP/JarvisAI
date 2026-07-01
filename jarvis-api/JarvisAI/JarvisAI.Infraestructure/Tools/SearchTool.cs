using JarvisAI.Domain.Interfaces;
using System.Text.Json;
using System.Web;

namespace JarvisAI.Infraestructure.Tools;

public class SearchTool : ITool
{
    public string Name => "SearchTool";
    public string Description => "Pesquisa informações na internet. Input: termo de pesquisa ex: últimas notícias de tecnologia";

    private readonly HttpClient _httpClient;

    public SearchTool()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "JarvisAI/1.0");
    }

    public async Task<string> ExecuteAsync(string input)
    {
        try
        {
            var query = HttpUtility.UrlEncode(input);
            var url = $"https://api.duckduckgo.com/?q={query}&format=json&no_html=1&skip_disambig=1";

            var response = await _httpClient.GetStringAsync(url);
            var json = JsonSerializer.Deserialize<JsonElement>(response);

            var abstract_ = json.GetProperty("Abstract").GetString();
            var answerText = json.GetProperty("Answer").GetString();
            var relatedTopics = json.GetProperty("RelatedTopics");

            var result = "";

            if (!string.IsNullOrEmpty(answerText))
                result += $"Resposta: {answerText}\n";

            if (!string.IsNullOrEmpty(abstract_))
                result += $"Resumo: {abstract_}\n";

            if (string.IsNullOrEmpty(result) && relatedTopics.GetArrayLength() > 0)
            {
                result = "Tópicos relacionados:\n";
                var count = 0;
                foreach (var topic in relatedTopics.EnumerateArray())
                {
                    if (count >= 3) break;
                    if (topic.TryGetProperty("Text", out var text))
                    {
                        result += $"- {text.GetString()}\n";
                        count++;
                    }
                }
            }

            return string.IsNullOrEmpty(result)
                ? $"Não encontrei resultados para '{input}'."
                : result;
        }
        catch
        {
            return $"Não consegui pesquisar sobre '{input}'.";
        }
    }
}