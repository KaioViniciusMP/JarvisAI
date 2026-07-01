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

            app.MapGet("/test-generate", async () =>
            {
                var httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };

                var requestBody = new
                {
                    model = "llama3.2",
                    prompt = "Olá!",
                    stream = false
                };

                var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("http://localhost:11434/api/generate", content);
                var result = await response.Content.ReadAsStringAsync();

                return Results.Ok(result);
            });
        }
    }
}
