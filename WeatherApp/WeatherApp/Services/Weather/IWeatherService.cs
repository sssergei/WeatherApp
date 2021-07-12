using System.Threading.Tasks;

namespace WeatherApp.Services.Weather
{
    public interface IWeatherService
    {
        Task<string> GetWeatherAsync(string zipcode);
    }
}
