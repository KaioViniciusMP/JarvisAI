using JarvisAI.Application.Interfaces;
using JarvisAI.Domain.Entities;
using JarvisAI.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System.ClientModel;

namespace JarvisAI.Infraestructure.Services;

public class ChatService : IChatService
{
    private readonly ChatClient _chatClient;
    private readonly JarvisDbContext _db;

    public ChatService(JarvisDbContext db, IConfiguration configuration)
    {
        var apiKey = configuration["Groq:ApiKey"]!;
        var model = configuration["Groq:Model"]!;

        _chatClient = new ChatClient(
            model: model,
            credential: new ApiKeyCredential(apiKey),
            options: new OpenAI.OpenAIClientOptions
            {
                Endpoint = new Uri("https://api.groq.com/openai/v1")
            }
        );

        _db = db;
    }

    public async Task<string> SendMessageAsync(string message, string user = "Kaio")
    {
        var history = await _db.Conversations
            .Where(c => c.User == user)
            .OrderByDescending(c => c.Date)
            .Take(10)
            .OrderBy(c => c.Date)
            .ToListAsync();

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage("Você é o Jarvis, um assistente inteligente. Responda sempre em português.")
        };

        foreach (var conv in history)
        {
            messages.Add(new UserChatMessage(conv.Question));
            messages.Add(new AssistantChatMessage(conv.Answer));
        }

        messages.Add(new UserChatMessage(message));

        var response = await _chatClient.CompleteChatAsync(messages);
        var answer = response.Value.Content[0].Text;

        _db.Conversations.Add(new Conversation
        {
            User = user,
            Question = message,
            Answer = answer,
            Date = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();

        return answer;
    }
}