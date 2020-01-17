using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RESTApiVersionSample.V3.Models;
using Newtonsoft.Json;

namespace RESTApiVersionSample.V3.Controllers
{
    [ApiController]
    [ApiVersion("3.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        }

        /// <summary>
        /// Gets a weather forecast.
        /// </summary>
        /// <returns>The requested weather forecast.</returns>
        /// <response code="200">The weather forecast was successfully retrieved.</response>
        /// <response code="404">There was an error when generating weather forecast.</response>
        [HttpGet]
        [Produces("application/json")]
        public IActionResult Get()
        {
            var rng = new Random();

            /*if (rng!=null)
            {
                return NotFound("test");
            }*/

            return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }));

        }

    }
}