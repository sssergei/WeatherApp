using System;
using System.Globalization;
using System.Reflection;
using TinyIoC;
using WeatherApp.Logger;
using WeatherApp.Services.Navigation;
using WeatherApp.Services.Weather;
using Xamarin.Forms;

namespace WeatherApp.ViewModels.Base
{
    public static class ViewModelLocator
    {
        private static TinyIoCContainer _container;

        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(ViewModelLocator.AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(ViewModelLocator.AutoWireViewModelProperty, value);
        }

        public static bool UseMockService { get; set; }

        static ViewModelLocator()
        {
            _container = new TinyIoCContainer();

            _container.Register<StartViewModel>();
            //_container.Register<SettingsViewModel>().AsSingleton();
            //_container.Register<DescriptionViewModel>();
            //_container.Register<WayViewModel>();
            //_container.Register<SelectWayViewModel>();
            //_container.Register<CleanSpaceViewModel>();

            _container.Register<INavigationService, NavigationService>();
            //_container.Register<IDependencyService, Services.Dependency.DependencyService>();
            _container.Register<IWeatherService, WeatherService>();
            //_container.Register<IDateService, DateService>();
            //_container.Register<IDataService, DataService>();
            //_container.Register<IJsonService, JsonService>();
            //_container.Register<IImportService, ImportService>();
            //_container.Register<IExportService, ExportService>();
            //_container.Register<IFileService, FileService>();
            _container.Register<ILogService, LogService>().AsSingleton();
            //_container.Register<IGpsService, GpsService>();
        }

        public static void RegisterSingleton<TInterface, T>() where TInterface : class where T : class, TInterface
        {
            _container.Register<TInterface, T>().AsSingleton();
        }

        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Element;
            if (view == null)
            {
                return;
            }

            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return;
            }
            var viewModel = _container.Resolve(viewModelType);
            view.BindingContext = viewModel;
        }
    }
}
