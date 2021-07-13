using WeatherApp.Data.Weather_Zone;

namespace WeatherApp.Services.Db
{
    public interface IDataService
    {
        bool AddWeather(WeatherZone weather, string zone);
        Data.Weather_Zone.WeatherZone GetWeather(string zone);
    }
}
