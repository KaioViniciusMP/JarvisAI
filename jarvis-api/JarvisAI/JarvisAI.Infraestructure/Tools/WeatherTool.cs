using JarvisAI.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace JarvisAI.Infraestructure.Tools;

public class WeatherTool : ITool
{
    public string Name => "WeatherTool";
    public string Description => "Consulta o clima de uma cidade. Input: nome da cidade em português ex: São Paulo";

    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public WeatherTool(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _apiKey = configuration["OpenWeather:ApiKey"]!;
    }

    public async Task<string> ExecuteAsync(string input)
    {
        try
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={input}&appid={_apiKey}&units=metric&lang=pt_br";

            var response = await _httpClient.GetStringAsync(url);
            var json = JsonSerializer.Deserialize<JsonElement>(response);

            var temp = json.GetProperty("main").GetProperty("temp").GetDouble();
            var feels = json.GetProperty("main").GetProperty("feels_like").GetDouble();
            var humidity = json.GetProperty("main").GetProperty("humidity").GetInt32();
            var description = json.GetProperty("weather")[0].GetProperty("description").GetString();
            var city = json.GetProperty("name").GetString();

            return $"Clima em {city}: {description}, temperatura {temp:F1}°C, sensação térmica {feels:F1}°C, umidade {humidity}%";
        }
        catch
        {
            return $"Não consegui obter o clima de {input}. Verifique o nome da cidade.";
        }
    }
}