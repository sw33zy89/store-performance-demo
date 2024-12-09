using PerformanceTesting.Core;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PerformanceTesting.Infrastructure.Services
{
    // Define classes to map the API response using System.Text.Json
    public class WeatherData
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("hourly_units")]
        public Dictionary<string, string> HourlyUnits { get; set; }

        [JsonPropertyName("hourly")]
        public HourlyData Hourly { get; set; }
    }

    public class HourlyData
    {
        [JsonPropertyName("time")]
        public List<string> Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public List<double> Temperature2m { get; set; }
    }

    public class OpenMeteoClient
    {
        private static readonly string BaseUrl = "https://api.open-meteo.com/v1/forecast";
        private readonly HttpClient _httpClient;

        public OpenMeteoClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Method to fetch weather data
        public async Task<WeatherData?> FetchWeatherDataAsync(GeoCoordinates coordinates)
        {
            var url = $"{BaseUrl}?latitude={coordinates.Latitude}&longitude={coordinates.Longitude}&hourly=temperature_2m";

            // Send GET request to the API
            var response = await _httpClient.GetStringAsync(url);

            // Deserialize the JSON response into the WeatherData class
            WeatherData? weatherData = JsonSerializer.Deserialize<WeatherData>(response);

            return weatherData;
        }
    }
}
