using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisAI.Application.Interfaces
{
    public interface IChatService
    {
        Task<string> SendMessageAsync(string message);
    }
}
