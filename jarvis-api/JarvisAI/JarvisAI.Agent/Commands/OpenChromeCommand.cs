using JarvisAI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisAI.Agent.Commands
{
    public class OpenChromeCommand : IAgentCommand
    {
        public string Name => "OpenChrome";
        public string Description => "Abre o Chrome. Parâmetros: URL (opcional)";

        public Task<string> ExecuteAsync(string parameters)
        {
            try
            {
                var args = string.IsNullOrEmpty(parameters) ? "" : parameters;
                Process.Start(new ProcessStartInfo
                {
                    FileName = "chrome",
                    Arguments = args,
                    UseShellExecute = true
                });
                return Task.FromResult($"Chrome aberto{(string.IsNullOrEmpty(parameters) ? "" : $" em {parameters}")}!");
            }
            catch
            {
                return Task.FromResult("Não consegui abrir o Chrome.");
            }
        }
    }
}
