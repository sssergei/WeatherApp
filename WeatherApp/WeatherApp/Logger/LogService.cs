using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherApp.Logger
{
    public class LogService : ILogService
    {
        static object _locker = new object();
        public async Task WriteLogAsync(string message)
        {
            var fileName = Path.Combine(App.FolderPath, "path.log");
            object locker = _locker;
            try
            {
                if (!File.Exists(fileName))
                {
                    File.Create(fileName).Close();
                }
                while (locker == null || Interlocked.CompareExchange(ref _locker, null, locker) != locker)
                {
                    await Task.Delay(3);
                    locker = _locker;
                }
                using (var streamWriter = new StreamWriter(fileName, true))
                {
                    await streamWriter.WriteLineAsync(message);
                }
                return;
            }
            catch
            {
                _locker = locker;
                return;
            }
            finally
            {
                _locker = locker;
            }
        }
        public async Task WriteLogAsync(Exception ex, string message)
        {
            var str = message + System.Environment.NewLine + ex.Message + ex.StackTrace + System.Environment.NewLine;
            await WriteLogAsync(str);
        }
        public void WriteLog(Exception ex, string message)
        {
            var str = message + System.Environment.NewLine + ex.Message + ex.StackTrace + System.Environment.NewLine;
            WriteLog(str);
        }
        public void WriteLog(string message)
        {
            var fileName = Path.Combine(App.FolderPath, "path.log");
            object locker = _locker;
            try
            {
                if (!File.Exists(fileName))
                {
                    File.Create(fileName).Close();
                }
                while (locker == null || Interlocked.CompareExchange(ref _locker, null, locker) != locker)
                {
                    Task.Delay(3);
                    locker = _locker;
                }
                using (var streamWriter = new StreamWriter(fileName, true))
                {
                    streamWriter.WriteLine(message);
                }
                return;
            }
            catch
            {
                _locker = locker;
                return;
            }
            finally
            {
                _locker = locker;
            }
        }

        public static void ErrorLog(string message)
        {
            var fileName = Path.Combine(App.FolderPath, "path.log");
            object locker = _locker;
            try
            {
                if (!File.Exists(fileName))
                {
                    File.Create(fileName).Close();
                }
                while (locker == null || Interlocked.CompareExchange(ref _locker, null, locker) != locker)
                {
                    Task.Delay(3);
                    locker = _locker;
                }
                using (var streamWriter = new StreamWriter(fileName, true))
                {
                    streamWriter.WriteLine(message);
                }
                return;
            }
            catch
            {
                _locker = locker;
                return;
            }
            finally
            {
                _locker = locker;
            }
        }
        public static void ErrorLog(Exception ex, string message)
        {
            var str = message + System.Environment.NewLine + ex.Message + ex.StackTrace + System.Environment.NewLine;
            ErrorLog(str);
        }
    }
}
