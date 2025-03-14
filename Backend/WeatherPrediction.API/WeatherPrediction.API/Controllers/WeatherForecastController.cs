using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherPrediction.API.Services;

namespace WeatherPrediction.API.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("forecast")]
        public async Task<IActionResult> GetForecast([FromQuery] string city, [FromQuery] bool offlineMode = false)
        {
            if (string.IsNullOrEmpty(city))
            {
                return BadRequest("City is required");
            }
            try
            {
                var forecast = await _weatherService.GetWeatherForecastAsync(city, offlineMode);
                return Ok(forecast);
            }
            

            catch (HttpRequestException ex)
            {
                return StatusCode(503, new { message = "External API is unreachable.", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}