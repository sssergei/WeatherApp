using System.Threading.Tasks;
using WeatherApp.ViewModels.Base;

namespace WeatherApp.Services.Navigation
{
    public interface INavigationService
    {
        ViewModelBase PreviousPageViewModel { get; }

        Task InitializeAsync();

        Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;

        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;

        Task RemoveLastFromBackStackAsync();

        Task RemoveBackStackAsync();
        Task MovePreviouseViewAsync(int backNumber = 1);
    }
}
