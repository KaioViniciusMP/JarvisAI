using JarvisAI.Application.Interfaces;
using JarvisAI.Domain.Entities;
using JarvisAI.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace JarvisAI.Infraestructure.Services;

public class ChatService : IChatService
{
    private readonly HttpClient _httpClient;
    private readonly JarvisDbContext _db;
    private const string Model = "qwen2.5:1.5b";
    private const string OllamaUrl = "http://localhost:11434/api/generate";

    public ChatService(JarvisDbContext db)
    {
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(5)
        };
        _db = db;
    }

    public async Task<string> SendMessageAsync(string message, string user = "Kaio")
    {
        // Busca histórico das últimas 10 conversas
        var history = await _db.Conversations
            .Where(c => c.User == user)
            .OrderByDescending(c => c.Date)
            .Take(10)
            .OrderBy(c => c.Date)
            .ToListAsync();

        // Monta contexto com histórico
        var context = "";
        foreach (var conv in history)
        {
            context += $"Usuário: {conv.Question}\nJarvis: {conv.Answer}\n";
        }

        var prompt = $"{context}Usuário: {message}\nJarvis:";

        var requestBody = new
        {
            model = Model,
            prompt = prompt,
            stream = false
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var httpResponse = await _httpClient.PostAsync(OllamaUrl, content);
        httpResponse.EnsureSuccessStatusCode();

        var responseJson = await httpResponse.Content.ReadAsStringAsync();
        var ollamaResponse = JsonSerializer.Deserialize<JsonElement>(responseJson);
        var response = ollamaResponse.GetProperty("response").GetString() ?? "";

        // Salva conversa no banco
        _db.Conversations.Add(new Conversation
        {
            User = user,
            Question = message,
            Answer = response,
            Date = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();

        return response;
    }
}