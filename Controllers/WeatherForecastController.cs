using Microsoft.AspNetCore.Mvc;

namespace DotnetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        private static WeatherForecast[] ListWatherForecast = [];

        public WeatherForecastController()
        {
            ListWatherForecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        /// <summary>
        /// Return the weather forecast at the specified position in the list. If the position is out of range, it returns a 404 Not Found response with a message indicating that there is no forecast at that position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>The element by position</returns>
        [HttpGet("{position}")]
        public ActionResult<WeatherForecast> GetByPosition(int position)
        {
           var element = ListWatherForecast.ElementAtOrDefault(position);
           if (element == null)
            {
               return NotFound($"No existe ningún pronóstico en la posición {position}.");
            }
            return Ok(element);
        }

        /// <summary>
        /// Deletes the weather forecast at the specified position in the list. If the position is out of range, it returns a 404 Not Found response with a message indicating that there is no forecast at that position. If the deletion is successful, it returns a 200 OK response with a message confirming the deletion.
        /// </summary>
        /// <param name="position"></param>
        /// <returns> 200 OK if the deletion is successful, or 404 Not Found if the position is out of range.</returns>
        [HttpDelete("{position}")]
        public ActionResult DeleteByPosition(int position) {
            var element = ListWatherForecast.ElementAtOrDefault(position);
            if (element == null)
            {
                return NotFound($"No existe ningún pronóstico en la posición {position}.");
            }
            ListWatherForecast = ListWatherForecast.Where((_, index) => index != position).ToArray();
            return Ok($"Se ha eliminado el registro en la posición {position}");
        }

        /// <summary>
        /// Updates the weather forecast at the specified position in the list with the provided updated forecast. If the position is out of range, it returns a 404 Not Found response with a message indicating that there is no forecast at that position. If the update is successful, it returns a 200 OK response with a message confirming the update.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="updatedForecast"></param>
        /// <returns>If the update is successful, it returns a 200 OK response with a message confirming the update. If the position is out of range, it returns a 404 Not Found response with a message indicating that there is no forecast at that position.</returns>
        [HttpPatch]
        public ActionResult UpdateWeatherForecast(int position, WeatherForecast updatedForecast)
        {
            var element = ListWatherForecast.ElementAtOrDefault(position);
            if (element == null)
            {
                return NotFound($"No existe ningún pronóstico en la posición {position}.");
            }
            ListWatherForecast[position] = updatedForecast;
            return Ok($"Se ha actualizado el registro en la posición {position}");
        }

        /// <summary>
        /// Return all weather forecasts in the list. Each forecast includes the date, temperature in Celsius and Fahrenheit, and a summary of the weather conditions. The data is generated randomly for demonstration purposes.
        /// </summary>
        /// <returns>List of Weather Forecast</returns>
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get([FromServices] ILogger<WeatherForecastController> logger)
        {
            logger.LogInformation("CONSOLE. Getting all weather forecasts.");
            return ListWatherForecast;
        }
    }
}
