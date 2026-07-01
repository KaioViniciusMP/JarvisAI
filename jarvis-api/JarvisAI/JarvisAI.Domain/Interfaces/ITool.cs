using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisAI.Domain.Interfaces
{
    public interface ITool
    {
        string Name { get; }
        string Description { get; }
        Task<string> ExecuteAsync(string input);
    }
}
