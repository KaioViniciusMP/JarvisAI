using JarvisAI.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisAI.Agent
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEnumerable<IAgentCommand> _commands;
        private HubConnection? _connection;
        private const string HubUrl = "http://localhost:5114/hubs/agent";

        public Worker(ILogger<Worker> logger, IEnumerable<IAgentCommand> commands)
        {
            _logger = logger;
            _commands = commands;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(HubUrl)
                .WithAutomaticReconnect()
                .Build();

            _connection.On<string, string>("ExecuteCommand", async (command, parameters) =>
            {
                _logger.LogInformation("Comando recebido: {Command} | Parâmetros: {Parameters}", command, parameters);

                var agentCommand = _commands.FirstOrDefault(c => c.Name == command);

                if (agentCommand is null)
                {
                    _logger.LogWarning("Comando não encontrado: {Command}", command);
                    await _connection.SendAsync("CommandResult", command, $"Comando '{command}' não encontrado.");
                    return;
                }

                var result = await agentCommand.ExecuteAsync(parameters);
                _logger.LogInformation("Resultado: {Result}", result);

                await _connection.SendAsync("CommandResult", command, result);
            });

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_connection.State == HubConnectionState.Disconnected)
                    {
                        await _connection.StartAsync(stoppingToken);
                        _logger.LogInformation("Agent conectado ao Hub!");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Erro ao conectar: {Error}", ex.Message);
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
