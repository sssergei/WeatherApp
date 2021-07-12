using System.Threading.Tasks;

namespace WeatherApp.Behaviors
{
	public interface IAction
	{
		Task<bool> Execute(object sender, object parameter);
	}
}
