using System;
using System.Threading.Tasks;

namespace WeatherApp.Logger
{
    public interface ILogService
    {
        Task WriteLogAsync(string message);
        Task WriteLogAsync(Exception ex, string message);
        void WriteLog(string message);
        void WriteLog(Exception ex, string message);
    }
}
