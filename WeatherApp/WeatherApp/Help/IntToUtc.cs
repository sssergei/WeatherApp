using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Help
{
    public static class IntToUtc
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static string GetUtc(int seconds)
        {
            return string.Format("{0:G}", UnixEpoch.AddSeconds(seconds).ToUniversalTime()) + " UTC";
        }
    }
}
