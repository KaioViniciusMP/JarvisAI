using Microsoft.AspNetCore.SignalR;

namespace JarvisAI.Api.Hubs
{
    public class AgentHub : Hub
    {
        public async Task SendCommand(string command, string parameters)
        {
            await Clients.All.SendAsync("ExecuteCommand", command, parameters);
        }

        public async Task CommandResult(string command, string result)
        {
            await Clients.All.SendAsync("ReceiveResult", command, result);
        }
    }
}
