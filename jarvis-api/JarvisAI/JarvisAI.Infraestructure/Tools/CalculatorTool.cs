using JarvisAI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisAI.Infraestructure.Tools
{
    public class CalculatorTool : ITool
    {
        public string Name => "CalculatorTool";
        public string Description => "Realiza cálculos matemáticos. Input: expressão matemática ex: 250 * 87";

        public Task<string> ExecuteAsync(string input)
        {
            try
            {
                var result = new DataTable().Compute(input, null);
                return Task.FromResult($"Resultado: {result}");
            }
            catch
            {
                return Task.FromResult("Não consegui calcular essa expressão.");
            }
        }
    }
}
