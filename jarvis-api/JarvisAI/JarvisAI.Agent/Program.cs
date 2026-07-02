using JarvisAI.Agent;
using JarvisAI.Agent.Commands;
using JarvisAI.Domain.Interfaces;

var builder = Host.CreateApplicationBuilder(args);

// Registra os comandos
builder.Services.AddSingleton<OpenVsCodeCommand>();
builder.Services.AddSingleton<OpenChromeCommand>();
builder.Services.AddSingleton<RunCommandPromptCommand>();

builder.Services.AddSingleton<IEnumerable<IAgentCommand>>(sp => new List<IAgentCommand>
{
    sp.GetRequiredService<OpenVsCodeCommand>(),
    sp.GetRequiredService<OpenChromeCommand>(),
    sp.GetRequiredService<RunCommandPromptCommand>()
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();