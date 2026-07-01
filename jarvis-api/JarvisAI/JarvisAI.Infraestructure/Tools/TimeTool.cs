using JarvisAI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisAI.Infraestructure.Tools
{
    public class TimeTool : ITool
    {
        public string Name => "TimeTool";
        public string Description => "Retorna a data e hora atual";

        public Task<string> ExecuteAsync(string input)
        {
            var now = DateTime.Now;
            var result = $"Agora são {now:HH:mm} do dia {now:dd/MM/yyyy}";
            return Task.FromResult(result);
        }
    }
}
