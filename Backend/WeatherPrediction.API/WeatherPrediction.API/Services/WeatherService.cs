using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherPrediction.API.Models;

namespace WeatherPrediction.API.Services
{
    public interface IWeatherService
    {
        Task<ForecastResult> GetWeatherForecastAsync(string city, bool offlineMode);
    }

    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://api.openweathermap.org/data/2.5/forecast";
        private readonly string _apiKey;
        private readonly string _offlineDataFile = "offline_weather.json";

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenWeatherAPIKey"];
        }

        public async Task<ForecastResult> GetWeatherForecastAsync(string city, bool offlineMode)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("City parameter cannot be null or empty.");
            }

            if (offlineMode && File.Exists(_offlineDataFile))
            {
                var offlineData = await File.ReadAllTextAsync(_offlineDataFile);
                var forecastListing = JsonSerializer.Deserialize<ForecastResult>(offlineData);
                if (forecastListing == null)
                {
                    throw new InvalidOperationException("Offline data is corrupted or empty.");
                }
                return forecastListing;
            }

            var response = await _httpClient.GetStringAsync($"{ApiUrl}?q={city}&appid={_apiKey}&units=metric&cnt=3");
            using var jsonDoc = JsonDocument.Parse(response);

            if (jsonDoc == null)
                throw new Exception("Failed to parse weather data.");

            var forecastData = jsonDoc.RootElement.GetProperty("list");
            var forecastList = forecastData.EnumerateArray().Select(f => new ForecastDetails
            {
                Date = DateTime.Parse(f.GetProperty("dt_txt").GetString()),
                HighTemp = f.GetProperty("main").GetProperty("temp_max").GetDouble(),
                LowTemp = f.GetProperty("main").GetProperty("temp_min").GetDouble(),
                Advice = GetAdvice(f)
            }).ToList();

            await File.WriteAllTextAsync(_offlineDataFile, JsonSerializer.Serialize(forecastList));

            return new ForecastResult
            {
                City = city,
                Forecast = forecastList
            };
        }

        private string GetAdvice(JsonElement weatherData)
        {
            var weatherConditions = weatherData.GetProperty("weather")[0].GetProperty("main").GetString();
            var highTemp = weatherData.GetProperty("main").GetProperty("temp_max").GetDouble();
            var windSpeed = weatherData.GetProperty("wind").GetProperty("speed").GetDouble();

            if (weatherConditions.Contains("Rain"))
                return "Carry umbrella";

            if (highTemp > 40)
                return "Use sunscreen lotion";

            if (windSpeed > 10)
                return "It's too windy, watch out!";

            if (weatherConditions.Contains("Thunderstorm"))
                return "Don’t step out! A Storm is brewing!";

            return "No special advice needed";
        }

    }
}
