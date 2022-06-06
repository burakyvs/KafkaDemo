using Confluent.Kafka;
using Kafka.Producer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Kafka.Producer.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly Random _random;
        private readonly KafkaProducerService _kafkaService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, KafkaProducerService kafkaService)
        {
            _logger = logger;
            _random = new Random();
            _kafkaService = kafkaService;
        }

        [HttpPost]
        public async Task<IActionResult> Produce()
        {
            try
            {
                // produce random object.
                var value = JsonSerializer.Serialize(new WeatherForecast { Date = DateTime.Now, Summary = Summaries[_random.Next(0, 10)], TemperatureC = _random.Next(-100, 100)});
                await _kafkaService.Producer.ProduceAsync("demo", new Message<Null, string>
                {
                    Value = value,
                });

                _kafkaService.Producer.Flush(TimeSpan.FromSeconds(10));

                return Ok(value);
            }
            catch (Exception ex)
            {
                _kafkaService.Producer?.Dispose();

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}