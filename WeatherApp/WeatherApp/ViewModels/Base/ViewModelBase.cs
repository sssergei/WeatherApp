using System.Threading.Tasks;
using System.Windows.Input;
using WeatherApp.Services.Navigation;
using Xamarin.Forms;

namespace WeatherApp.ViewModels.Base
{
    public abstract class ViewModelBase : ExtendedBindableObject
    {
        protected readonly INavigationService NavigationService;

        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                if (_isBusy == value)
                { return; }
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public ViewModelBase()
        {
            NavigationService = ViewModelLocator.Resolve<INavigationService>();
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

        #region PAGE EVENTS

        public ICommand PageAppearingCommand => new Command(async () => await PageAppearingCommandAsync());

        protected virtual async Task PageAppearingCommandAsync()
        {
            await Task.Delay(1);
        }
        public ICommand PageDisappearingCommand => new Command(async () => await PageDisappearingCommandAsync());

        protected virtual async Task PageDisappearingCommandAsync()
        {
            await Task.Delay(1);
        }

        #endregion
    }
}
