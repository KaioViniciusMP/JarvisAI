using JarvisAI.Application.Interfaces;
using JarvisAI.Domain.Entities;
using JarvisAI.Domain.Interfaces;
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
    private readonly IToolService _toolService;
    private readonly IEnumerable<ITool> _tools;

    public ChatService(JarvisDbContext db, IConfiguration configuration, IToolService toolService, IEnumerable<ITool> tools)
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
        _toolService = toolService;
        _tools = tools;
    }

    public async Task<string> SendMessageAsync(string message, string user = "Kaio")
    {
        // Busca histórico
        var history = await _db.Conversations
            .Where(c => c.User == user)
            .OrderByDescending(c => c.Date)
            .Take(10)
            .OrderBy(c => c.Date)
            .ToListAsync();

        // Monta lista de ferramentas disponíveis
        var toolsDescription = string.Join("\n", _tools.Select(t => $"- {t.Name}: {t.Description}"));

        // Monta mensagens
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage($"""
                   Você é o Jarvis, um assistente inteligente. Responda sempre em português.
                   
                   Você tem acesso às seguintes ferramentas:
                   {toolsDescription}
                   
                   REGRAS OBRIGATÓRIAS:
                   - Se o usuário perguntar horas, data ou hora atual, use OBRIGATORIAMENTE a TimeTool
                   - Se o usuário pedir um cálculo matemático, use OBRIGATORIAMENTE a CalculatorTool
                   - Quando usar ferramenta, responda APENAS neste formato exato:
                   TOOL: NomeDaFerramenta
                   INPUT: entrada
                   
                   Nunca tente responder horas ou cálculos sem usar as ferramentas.
                   """)
        };

        foreach (var conv in history)
        {
            messages.Add(new UserChatMessage(conv.Question));
            messages.Add(new AssistantChatMessage(conv.Answer));
        }

        messages.Add(new UserChatMessage(message));

        // Primeira chamada para IA decidir se usa ferramenta
        var firstResponse = await _chatClient.CompleteChatAsync(messages);
        var firstAnswer = firstResponse.Value.Content[0].Text;

        string finalAnswer;

        // Verifica se a IA quer usar uma ferramenta
        if (firstAnswer.StartsWith("TOOL:"))
        {
            var lines = firstAnswer.Split('\n');
            var toolName = lines[0].Replace("TOOL:", "").Trim();
            var toolInput = lines.Length > 1 ? lines[1].Replace("INPUT:", "").Trim() : "";

            // Executa a ferramenta
            var toolResult = await _toolService.ExecuteToolAsync(toolName, toolInput);

            // Segunda chamada com o resultado da ferramenta
            messages.Add(new AssistantChatMessage(firstAnswer));
            messages.Add(new UserChatMessage($"Resultado da ferramenta {toolName}: {toolResult}. Agora responda ao usuário de forma natural."));

            var secondResponse = await _chatClient.CompleteChatAsync(messages);
            finalAnswer = secondResponse.Value.Content[0].Text;
        }
        else
        {
            finalAnswer = firstAnswer;
        }

        // Salva conversa
        _db.Conversations.Add(new Conversation
        {
            User = user,
            Question = message,
            Answer = finalAnswer,
            Date = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();

        return finalAnswer;
    }
}