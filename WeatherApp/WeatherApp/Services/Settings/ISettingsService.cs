using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Services.Settings
{
    public interface ISettingsService
    {
        bool AskWhenDeleteExportFiles { get; set; }
        bool AskWhenDeleteImportFiles { get; set; }
        bool AskWhenDeleteWaysPhoto { get; set; }
        bool AskWhenDeleteCurrentPhoto { get; set; }
        bool ScreenOn { get; set; }
        float GpsAccuracy { get; set; }
        //PhotoSize PhotoSize { get; set; }
        bool Disproportionate { get; set; }

        float GetValueOrDefault(string key, float defaultValue);
        bool GetValueOrDefault(string key, bool defaultValue);
        string GetValueOrDefault(string key, string defaultValue);
        //PhotoSize GetValueOrDefault(string key, PhotoSize defaultValue);
        Task AddOrUpdateValue(string key, bool value);
        Task AddOrUpdateValue(string key, string value);
        Task AddOrUpdateValue(string key, float value);
        //Task AddOrUpdateValue(string key, PhotoSize value);
    }
}
