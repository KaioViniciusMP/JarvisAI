using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisAI.Domain.Interfaces
{
    public interface IToolService
    {
        Task<string> ExecuteToolAsync(string toolName, string input);
        IEnumerable<string> GetAvailableTools();
    }
}
