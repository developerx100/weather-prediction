using Moq;
using System.Text.Json;
using WeatherPrediction.API.Services;
using Microsoft.Extensions.Configuration;
using WeatherPrediction.API.Models;

public class WeatherServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly WeatherService _weatherService;

    public WeatherServiceTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _configurationMock = new Mock<IConfiguration>();
        var httpClient = new HttpClient();
        _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
        _weatherService = new WeatherService(httpClient, _configurationMock.Object);
    }

    [Fact]
    public async Task GetWeatherAsync_ShouldThrowException_WhenInvalidCityProvided()
    {
        // Arrange
        string city = "";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _weatherService.GetWeatherForecastAsync(city, false));
    }

    [Fact]
    public async Task GetWeatherAsync_ShouldReturnOfflineData_WhenOfflineModeIsTrue()
    {
        // Arrange
        string city = "London";
        bool offlineMode = true;

        var expectedData = new ForecastResult {
            City = city,
            Forecast = new List<ForecastDetails> {
                new ForecastDetails {
                    Date = new DateTime(2025, 3, 14), HighTemp = 30, LowTemp = 20, Advice = "Carry umbrella" 
                }
            }
            
        };

        File.WriteAllText("offline_weather.json", JsonSerializer.Serialize(expectedData));

        // Act
        var result = await _weatherService.GetWeatherForecastAsync(city, offlineMode);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedData.Forecast.Count, result.Forecast.Count);
        Assert.Equal(expectedData.Forecast[0].Advice, result.Forecast[0].Advice);
    }
}
