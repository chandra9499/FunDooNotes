using Microsoft.AspNetCore.Mvc;

namespace FunDooNotes.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _logger.Log(LogLevel.Information,"hello this is weather controller");
            _logger.LogInformation("hello again from weather conyroller");
            _logger.LogWarning("this is a warning while weather forcasting");
            try
            {
                throw new Exception("");
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex,"an exception occred during running");
            }
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation(101,"getting weather forecast of {count} days",5);
            string p1 = "param1";
            string p2 = "param2";
            _logger.LogInformation("parameter value :{p1},{p2}",p1,p2);
            _logger.LogTrace("this is trace message");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
