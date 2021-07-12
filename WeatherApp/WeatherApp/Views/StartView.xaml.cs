using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartView : ContentPage
    {
        public StartView()
        {
            InitializeComponent();
        }
        //protected override void OnAppearing()
        //{
        //    NavigationPage.SetHasNavigationBar((Xamarin.Forms.Application.Current.MainPage as NavigationPage).CurrentPage, false);
        //}
    }
}