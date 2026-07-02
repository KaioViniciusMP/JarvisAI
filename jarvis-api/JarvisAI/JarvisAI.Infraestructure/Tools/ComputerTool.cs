using JarvisAI.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;



namespace JarvisAI.Infraestructure.Tools
{
    public class ComputerTool : ITool
    {
        public string Name => "ComputerTool";
        public string Description => "Controla o computador. Input: 'OpenVsCode: C:\\Users\\LENOVO\\Documents\\JarvisAI\\jarvis-api\\JarvisAI', 'OpenChrome: https://www.google.com/?hl=pt_BR&zx=1783018181724', 'RunCommand: comando'";

        private readonly HubConnection _connection;

        public ComputerTool()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5114/hubs/agent")
                .WithAutomaticReconnect()
                .Build();
        }

        public async Task<string> ExecuteAsync(string input)
        {
            try
            {
                if (_connection.State == HubConnectionState.Disconnected)
                    await _connection.StartAsync();

                var parts = input.Split(':', 2);
                var command = parts[0].Trim();
                var parameters = parts.Length > 1 ? parts[1].Trim() : "";

                var result = "";

                _connection.On<string, string>("ReceiveResult", (cmd, res) =>
                {
                    result = res;
                });

                await _connection.SendAsync("SendCommand", command, parameters);

                await Task.Delay(2000);

                return string.IsNullOrEmpty(result)
                    ? $"Comando '{command}' enviado para o computador!"
                    : result;
            }
            catch (Exception ex)
            {
                return $"Erro ao executar comando: {ex.Message}";
            }
        }
    }
}
