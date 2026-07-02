using JarvisAI.Api.Endpoints;
using JarvisAI.Api.Hubs;
using JarvisAI.Application.Interfaces;
using JarvisAI.Domain.Interfaces;
using JarvisAI.Infraestructure.Data;
using JarvisAI.Infraestructure.Services;
using JarvisAI.Infraestructure.Tools;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddDbContext<JarvisDbContext>(options =>
    options.UseSqlite("Data Source=jarvis.db"));

// Registra as ferramentas
builder.Services.AddScoped<TimeTool>();
builder.Services.AddScoped<CalculatorTool>();
builder.Services.AddScoped<WeatherTool>();
builder.Services.AddScoped<SearchTool>();
builder.Services.AddScoped<NewsTool>();
builder.Services.AddScoped<ReminderTool>();
builder.Services.AddScoped<ComputerTool>();

// Registra IEnumerable<ITool>
builder.Services.AddScoped<IEnumerable<ITool>>(sp => new List<ITool>
{
    sp.GetRequiredService<TimeTool>(),
    sp.GetRequiredService<CalculatorTool>(),
    sp.GetRequiredService<WeatherTool>(),
    sp.GetRequiredService<SearchTool>(),
    sp.GetRequiredService<NewsTool>(),
    sp.GetRequiredService<ReminderTool>(),
    sp.GetRequiredService<ComputerTool>()
});

builder.Services.AddScoped<IToolService, ToolService>();
builder.Services.AddScoped<IChatService, ChatService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapChatEndpoints();
app.MapHub<AgentHub>("/hubs/agent");

app.Run();