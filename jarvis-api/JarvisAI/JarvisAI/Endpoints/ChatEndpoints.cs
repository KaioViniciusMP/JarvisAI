using JarvisAI.Application.DTOs;

namespace JarvisAI.Api.Endpoints
{
    public static class ChatEndpoints
    {
        public static void MapChatEndpoints(this WebApplication app)
        {
            app.MapPost("/chat", (ChatRequest request) =>
            {
                var response = new ChatResponse
                {
                    Response = $"Você disse: {request.Message}"
                };

                return Results.Ok(response);
            });
        }
    }
}
