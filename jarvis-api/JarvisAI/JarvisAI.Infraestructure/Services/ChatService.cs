using JarvisAI.Application.Interfaces;
using OllamaSharp;
using OllamaSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisAI.Infraestructure.Services
{
    public class ChatService : IChatService
    {
        private readonly OllamaApiClient _ollama;
        private const string Model = "llama3.2";

        public ChatService()
        {
            _ollama = new OllamaApiClient("http://localhost:11434");
        }

        public async Task<string> SendMessageAsync(string message)
        {
            var response = "";

            var request = new GenerateRequest
            {
                Model = Model,
                Prompt = message
            };

            await foreach (var chunk in _ollama.GenerateAsync(request))
            {
                response += chunk?.Response;
            }

            return response;
        }
    }
}
