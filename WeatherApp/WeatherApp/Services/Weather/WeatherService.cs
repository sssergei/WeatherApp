using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Logger;

namespace WeatherApp.Services.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient client = new HttpClient();
        private string country = "us";
        private string api = "13ab90127845f75d4ee40afe1ee27571";
        private ILogService _logService;

        public WeatherService(ILogService logService)
        {
            _logService = logService;
        }
        public async Task<string> GetWeatherAsync(string zipcode)
        {
            try
            {
                var uriWeather = $"https://api.openweathermap.org/data/2.5/weather?zip={zipcode},{country}&units=imperial&appid={api}";
                HttpResponseMessage response = await client.GetAsync(uriWeather).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await _logService.WriteLogAsync(ex, "async Task GetWeatherAsync()");
                return string.Empty;
            }
        }
    }
}
