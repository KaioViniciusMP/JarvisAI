using JarvisAI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JarvisAI.Agent.Commands
{
    public class OpenVsCodeCommand : IAgentCommand
    {
        public string Name => "OpenVsCode";
        public string Description => "Abre o Visual Studio Code. Parâmetros: caminho do projeto (opcional)";

        public Task<string> ExecuteAsync(string parameters)
        {
            try
            {
                var args = string.IsNullOrEmpty(parameters) ? "" : $"\"{parameters}\"";
                Process.Start(new ProcessStartInfo
                {
                    FileName = "code",
                    Arguments = args,
                    UseShellExecute = true
                });
                return Task.FromResult($"VS Code aberto{(string.IsNullOrEmpty(parameters) ? "" : $" no projeto {parameters}")}!");
            }
            catch
            {
                return Task.FromResult("Não consegui abrir o VS Code.");
            }
        }
    }
}
