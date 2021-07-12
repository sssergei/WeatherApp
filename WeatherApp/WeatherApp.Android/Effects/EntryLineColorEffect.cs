using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WeatherApp.Behaviors;
using WeatherApp.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("WeatherApp")]
[assembly: ExportEffect(typeof(EntryLineColorEffect), "EntryLineColorEffect")]
namespace WeatherApp.Droid.Effects
{
    public class EntryLineColorEffect : PlatformEffect
    {
        EditText control;


        protected override void OnAttached()
        {
            try
            {
                control = Control as EditText;
                UpdateLineColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
            control = null;
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == LineColorBehavior.LineColorProperty.PropertyName)
            {
                UpdateLineColor();
            }
        }

        private void UpdateLineColor()
        {
            try
            {
                if (control != null)
                {
                    //control.Background.SetColorFilter(Android.Graphics.BlendModeColorFilter.
                    //control.Background.SetColorFilter(LineColorBehavior.GetLineColor(Element).ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcAtop);

                    control.Background.SetColorFilter(new BlendModeColorFilter(LineColorBehavior.GetLineColor(Element).ToAndroid(), BlendMode.SrcAtop));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}