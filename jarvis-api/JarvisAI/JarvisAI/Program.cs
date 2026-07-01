using JarvisAI.Api.Endpoints;
using JarvisAI.Application.Interfaces;
using JarvisAI.Infraestructure.Data;
using JarvisAI.Infraestructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddDbContext<JarvisDbContext>(options =>
    options.UseSqlite("Data Source=jarvis.db"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapChatEndpoints();

app.Run();