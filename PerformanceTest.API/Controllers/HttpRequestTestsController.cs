using Microsoft.AspNetCore.Mvc;
using PerformanceTesting.Core;
using PerformanceTesting.Infrastructure.Persistence;
using PerformanceTesting.Infrastructure.Services;

namespace PerformanceTest.API.Controllers
{
    [Route("api/httptests")]
    [ApiController]
    public class HttpRequestTestsController : ControllerBase
    {
        private readonly OpenMeteoClient _client;
        public HttpRequestTestsController(OpenMeteoClient openMeteoClient)
        {
            _client = openMeteoClient;
        }

        [HttpGet]
        [Route("sync/{numRequests}")]
        public ActionResult FetchRequestsSync(int numRequests)
        {
            List<WeatherData?> weatherData = new List<WeatherData?>();
            for (int i = 0; i < numRequests; i++)
            {
                Address address = DataGenerator.GenerateRandomAddress();
                weatherData.Add(_client.FetchWeatherDataAsync(address.Coordinates).GetAwaiter().GetResult());
            }

            return Ok(weatherData.Count);
        }

        [HttpGet]
        [Route("async/{numRequests}")]
        public async Task<ActionResult> FetchRequestsAsync(int numRequests)
        {
            List<WeatherData?> weatherData = new List<WeatherData?>();
            for (int i = 0; i < numRequests; i++)
            {
                Address address = DataGenerator.GenerateRandomAddress();
                var result = await _client.FetchWeatherDataAsync(address.Coordinates);
                weatherData.Add(result);
            }

            return Ok(weatherData.Count);
        }

        [HttpGet]
        [Route("async-whenall/{numRequests}")]
        public async Task<ActionResult> FetchRequestsAsyncWhenAll(int numRequests)
        {
            List<Task<WeatherData?>> tasks = new List<Task<WeatherData?>>();
            List<WeatherData?> weatherData = new List<WeatherData?>();
            for (int i = 0; i < numRequests; i++)
            {
                Address address = DataGenerator.GenerateRandomAddress();
                tasks.Add(_client.FetchWeatherDataAsync(address.Coordinates));
            }

            weatherData.AddRange(await Task.WhenAll(tasks));
            return Ok(weatherData.Count);
        }
    }
}
