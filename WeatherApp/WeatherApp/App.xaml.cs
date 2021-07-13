using WeatherApp.Views;
using Xamarin.Forms;

namespace WeatherApp
{
    public partial class App : Application
    {
        public static bool HasPermission { get; set; }
        public static string FolderPath { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new StartView();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
