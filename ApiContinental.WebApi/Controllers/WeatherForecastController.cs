using Microsoft.AspNetCore.Mvc;

namespace ApiContinental.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                         IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration; 
        }

        // GET /WeatherForecast
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        // GET /WeatherForecast/secret  <-- ruta distinta para evitar conflicto Swagger
        [HttpGet("secret", Name = "Obtener-secreto")]
        public IActionResult GetSecret()
        {
            var secretValue = _configuration["DbConnectionString"];
            if (string.IsNullOrEmpty(secretValue))
            {
                return NotFound("No se encontró el secreto.");
            }
            return Ok(secretValue);
        }
    }
}
