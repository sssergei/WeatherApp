using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using WeatherApp.ViewModels.Base;
using WeatherApp.Logger;
using WeatherApp.Services.Weather;
using WeatherApp.Validations;
using Newtonsoft.Json;
using WeatherApp.Data.Weather_Zone;
using WeatherApp.Help;
using WeatherApp.Services.Db;

namespace WeatherApp.ViewModels
{
    public class StartViewModel : ViewModelBase
    {
        ILogService _logService;
        IWeatherService _weatherService;
        IDataService _dataService;

        public StartViewModel(IWeatherService weatherService, IDataService dataService, ILogService logService)
        {
            _weatherService = weatherService;
            _logService = logService;
            _dataService = dataService;
            _zipCode = new ValidatableObject<string>();
        }

        #region PROPERTIES

        ValidatableObject<string> _zipCode;
        public ValidatableObject<string> ZipCode
        {
            get { return _zipCode; }
            set
            {
                _zipCode = value;
                if (_zipCode != null && _zipCode.Value != null)
                {
                    _zipCode.Value = _zipCode.Value.Trim();
                }
                RaisePropertyChanged(() => ZipCode);
            }
        }

        ValidatableObject<string> _location;
        public ValidatableObject<string> Location
        {
            get { return _location; }
            set
            {
                _location = value;
                if (_location != null && _location.Value != null)
                {
                    _location.Value = _location.Value.Trim();
                }
                RaisePropertyChanged(() => Location);
            }
        }

        ValidatableObject<string> _temperature;
        public ValidatableObject<string> Temperature
        {
            get { return _temperature; }
            set
            {
                _temperature = value;
                if (_temperature != null && _temperature.Value != null)
                {
                    _temperature.Value = _temperature.Value.Trim();
                }
                RaisePropertyChanged(() => Temperature);
            }
        }
        ValidatableObject<string> _windSpeed;
        public ValidatableObject<string> WindSpeed
        {
            get { return _windSpeed; }
            set
            {
                _windSpeed = value;
                if (_windSpeed != null && _windSpeed.Value != null)
                {
                    _windSpeed.Value = _windSpeed.Value.Trim();
                }
                RaisePropertyChanged(() => WindSpeed);
            }
        }
        ValidatableObject<string> _humidity;
        public ValidatableObject<string> Humidity
        {
            get { return _humidity; }
            set
            {
                _humidity = value;
                if (_humidity != null && _humidity.Value != null)
                {
                    _humidity.Value = _humidity.Value.Trim();
                }
                RaisePropertyChanged(() => Humidity);
            }
        }

        ValidatableObject<string> _visibility;
        public ValidatableObject<string> Visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                if (_visibility != null && _visibility.Value != null)
                {
                    _visibility.Value = _visibility.Value.Trim();
                }
                RaisePropertyChanged(() => Visibility);
            }
        }

        ValidatableObject<string> _timeofSunrise;
        public ValidatableObject<string> TimeofSunrise
        {
            get { return _timeofSunrise; }
            set
            {
                _timeofSunrise = value;
                if (_timeofSunrise != null && _timeofSunrise.Value != null)
                {
                    _timeofSunrise.Value = _timeofSunrise.Value.Trim();
                }
                RaisePropertyChanged(() => TimeofSunrise);
            }
        }
        ValidatableObject<string> _timeofSunset;
        public ValidatableObject<string> TimeofSunset
        {
            get { return _timeofSunset; }
            set
            {
                _timeofSunset = value;
                if (_timeofSunset != null && _timeofSunset.Value != null)
                {
                    _timeofSunset.Value = _timeofSunset.Value.Trim();
                }
                RaisePropertyChanged(() => TimeofSunset);
            }
        }

        #endregion

        #region ICommand
        public ICommand GetWeatherCommand => new Command(async () => await GetWeatherAsync());
        public ICommand ValidateZipCommand => new Command(() => ValidateZipCode());
        #endregion

        bool _isEnableWeatherButton = true;
        public bool IsEnableWeatherButton
        {
            get
            {
                return _isEnableWeatherButton;
            }
            set
            {
                _isEnableWeatherButton = value;
                RaisePropertyChanged(() => IsEnableWeatherButton);
            }
        }

        private bool ValidateZipCode()
        {
            var r = _zipCode.Validate();
            if (string.IsNullOrEmpty(_zipCode.Value) || _zipCode.Value.Length <= 50)
            { IsEnableWeatherButton = r; }
            return IsEnableWeatherButton;
        }

        /// <summary>
        /// Method call the GetWeatherAsync .
        /// </summary>
        private async Task GetWeatherAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var responce = await _weatherService.GetWeatherAsync(ZipCode.Value);
                if (string.IsNullOrEmpty(responce.Trim()))
                {
                    WeatherZone d = _dataService.GetWeather(ZipCode.Value);
                    SetData(d);
                    return;
                }
                var weather = JsonConvert.DeserializeObject<WeatherZone>(responce);
                if (weather == null) return;
                SetData(weather);
                _dataService.AddWeather(weather, ZipCode.Value);
            }
            catch (Exception ex)
            {
                await _logService.WriteLogAsync(ex, "async Task GetWeatherAsync()");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private void SetData(WeatherZone wzone)
        {
            if (Location == null) Location = new ValidatableObject<string>();
            Location.Value = wzone.name;
            if (Temperature == null) Temperature = new ValidatableObject<string>();
            Temperature.Value = wzone.main.temp.ToString() + "F";
            if (WindSpeed == null) WindSpeed = new ValidatableObject<string>();
            WindSpeed.Value = wzone.wind.speed.ToString() + "mph";
            if (Humidity == null) Humidity = new ValidatableObject<string>();
            Humidity.Value = wzone.main.humidity.ToString() + "%";
            if (Visibility == null) Visibility = new ValidatableObject<string>();
            Visibility.Value = wzone.weather[0].main;
            if (TimeofSunrise == null) TimeofSunrise = new ValidatableObject<string>();
            TimeofSunrise.Value = IntToUtc.GetUtc(wzone.sys.sunset);
            if (TimeofSunset == null) TimeofSunset = new ValidatableObject<string>();
            TimeofSunset.Value = IntToUtc.GetUtc(wzone.sys.sunset);
        }
    }
}
