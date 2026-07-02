using JarvisAI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisAI.Agent.Commands
{
    public class RunCommandPromptCommand : IAgentCommand
    {
        public string Name => "RunCommand";
        public string Description => "Executa um comando no terminal. Parâmetros: comando a executar";

        public Task<string> ExecuteAsync(string parameters)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c {parameters}",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                return Task.FromResult(string.IsNullOrEmpty(output) ? error : output);
            }
            catch
            {
                return Task.FromResult("Não consegui executar o comando.");
            }
        }
    }
}
