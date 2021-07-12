using System;
using System.Threading.Tasks;
using WeatherApp.Logger;
using Xamarin.Forms;

namespace WeatherApp.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        ILogService _logService;

        #region Setting Constants

        private const string DeleteExport = "delete_export";
        private readonly bool DeleteExportDefault = true;
        private const string DeleteImport = "delete_import";
        private readonly bool DeleteImportDefault = true;
        private const string Accuracy = "gps_accuracy";
        private readonly float AccuracyDefault = 8f;
        private const string DeleteWayPhoto = "delete_photo";
        private readonly bool DeleteWayPhotoDefault = true;
        private const string DeleteCurrentPhoto = "delete_currphoto";
        private readonly bool DeleteCurrentPhotoDefault = true;
        private const string SetScreenOn = "set_screenon";
        private readonly bool SetScreenOnDefault = false;
        private const string SizePhoto = "size_photo";
        private readonly string Disprop = "Disprop";
        private readonly bool DispropDefault = true;
        //private const PhotoSize DefaultSizePhoto = Plugin.Media.Abstractions.PhotoSize.Full;


        #endregion

        public SettingsService(ILogService logService)
        {
            _logService = logService;
        }

        #region Settings Properties

        //public PhotoSize PhotoSize
        //{
        //    get => GetValueOrDefault(SizePhoto, DefaultSizePhoto);
        //    set => AddOrUpdateValue(SizePhoto, value);
        //}
        public float GpsAccuracy
        {
            get => GetValueOrDefault(Accuracy, AccuracyDefault);
            set => AddOrUpdateValue(Accuracy, value);
        }
        public bool ScreenOn
        {
            get => GetValueOrDefault(SetScreenOn, SetScreenOnDefault);
            set => AddOrUpdateValue(SetScreenOn, value);
        }

        public bool AskWhenDeleteImportFiles
        {
            get => GetValueOrDefault(DeleteImport, DeleteImportDefault);
            set => AddOrUpdateValue(DeleteImport, value);
        }
        public bool AskWhenDeleteExportFiles
        {
            get => GetValueOrDefault(DeleteExport, DeleteExportDefault);
            set => AddOrUpdateValue(DeleteExport, value);
        }

        public bool AskWhenDeleteWaysPhoto
        {
            get => GetValueOrDefault(DeleteWayPhoto, DeleteWayPhotoDefault);
            set => AddOrUpdateValue(DeleteWayPhoto, value);
        }

        public bool AskWhenDeleteCurrentPhoto
        {
            get => GetValueOrDefault(DeleteCurrentPhoto, DeleteCurrentPhotoDefault);
            set => AddOrUpdateValue(DeleteCurrentPhoto, value);
        }

        public bool Disproportionate
        {
            get => GetValueOrDefault(Disprop, DispropDefault);
            set => AddOrUpdateValue(Disprop, value);
        }
        #endregion

        #region Public Methods

        public Task AddOrUpdateValue(string key, float value) => AddOrUpdateValueInternal(key, value);
        //public Task AddOrUpdateValue(string key, PhotoSize value) => AddOrUpdateValueInternal(key, value);
        public Task AddOrUpdateValue(string key, bool value) => AddOrUpdateValueInternal(key, value);
        public Task AddOrUpdateValue(string key, string value) => AddOrUpdateValueInternal(key, value);
        public bool GetValueOrDefault(string key, bool defaultValue) => GetValueOrDefaultInternal(key, defaultValue);
        //public PhotoSize GetValueOrDefault(string key, PhotoSize defaultValue) => GetValueOrDefaultInternal(key, defaultValue);
        public string GetValueOrDefault(string key, string defaultValue) => GetValueOrDefaultInternal(key, defaultValue);
        public float GetValueOrDefault(string key, float defaultValue) => GetValueOrDefaultInternal(key, defaultValue);

        #endregion

        #region Internal Implementation

        async Task AddOrUpdateValueInternal<T>(string key, T value)
        {
            if (value == null)
            {
                await Remove(key);
            }

            //if (key == "size_photo")
            //{
            //    if (value is PhotoSize)
            //    {
            //        var v = value.ToString();
            //        Application.Current.Properties[key] = v;
            //    }
            //}
            //else
            //{
            //    Application.Current.Properties[key] = value;
            //}
            Application.Current.Properties[key] = value;
            try
            {
                await Application.Current.SavePropertiesAsync();
            }
            catch (Exception ex)
            {
                await _logService.WriteLogAsync(ex, "Task AddOrUpdateValueInternal<T>(string key, T value)");
            }
        }

        T GetValueOrDefaultInternal<T>(string key, T defaultValue = default(T))
        {
            object value = null;
            if (Application.Current.Properties.ContainsKey(key))
            {
                //if (key == "size_photo")
                //{
                //    var v = (string)Application.Current.Properties[key];
                //    value = (PhotoSize)Enum.Parse(typeof(PhotoSize), v, true);
                //}
                //else
                //{
                //    value = Application.Current.Properties[key];
                //}
                value = Application.Current.Properties[key];
            }
            return null != value ? (T)value : defaultValue;
        }

        async Task Remove(string key)
        {
            try
            {
                if (Application.Current.Properties[key] != null)
                {
                    Application.Current.Properties.Remove(key);
                    await Application.Current.SavePropertiesAsync();
                }
            }
            catch (Exception ex)
            {
                await _logService.WriteLogAsync($"Unable to remove: { key}, Message: {ex.Message}");
            }
        }

        #endregion
    }
}
