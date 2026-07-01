using JarvisAI.Application.DTOs;
using JarvisAI.Application.Interfaces;
using JarvisAI.Domain.Interfaces;

namespace JarvisAI.Api.Endpoints
{
    public static class ChatEndpoints
    {
        public static void MapChatEndpoints(this WebApplication app)
        {
            app.MapPost("/chat", async (ChatRequest request, IChatService chatService) =>
            {
                var response = await chatService.SendMessageAsync(request.Message);
                return Results.Ok(new ChatResponse { Response = response });
            });

            app.MapGet("/tools", (IToolService toolService) =>
            {
                var tools = toolService.GetAvailableTools();
                return Results.Ok(tools);
            });

            app.MapPost("/tools/{toolName}", async (string toolName, ChatRequest request, IToolService toolService) =>
            {
                var result = await toolService.ExecuteToolAsync(toolName, request.Message);
                return Results.Ok(new ChatResponse { Response = result });
            });
        }
    }
}
