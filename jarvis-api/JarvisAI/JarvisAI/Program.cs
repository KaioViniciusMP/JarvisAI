using JarvisAI.Api.Endpoints;
using JarvisAI.Application.Interfaces;
using JarvisAI.Infraestructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<IChatService, ChatService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.MapChatEndpoints();

app.Run();