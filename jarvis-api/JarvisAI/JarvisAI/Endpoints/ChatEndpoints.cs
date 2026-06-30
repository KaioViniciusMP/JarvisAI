using JarvisAI.Application.DTOs;
using JarvisAI.Application.Interfaces;

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
        }
    }
}
