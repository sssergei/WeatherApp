using Xamarin.Forms;

[assembly: Dependency(typeof(WeatherApp.Droid.Dependency.FileSystemImplementation))]
namespace WeatherApp.Droid.Dependency
{
    public class FileSystemImplementation : IFileSystem
    {
        public string GetExternalStorage()
        {
            return System.IO.Path.Combine(Android.App.Application.Context.GetExternalFilesDir("").AbsolutePath);
        }
    }
    public interface IFileSystem
    {
        string GetExternalStorage();
    }
}