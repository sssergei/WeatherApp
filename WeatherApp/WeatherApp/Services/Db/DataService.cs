using SQLite;
using System;
using System.IO;
using System.Linq;
using WeatherApp.Services.Db.DataModels;
using WeatherApp.Logger;
using System.Globalization;

namespace WeatherApp.Services.Db
{
    public class DataService : IDataService
    {
        public static readonly string DbPath = Path.Combine(App.FolderPath, "WeatherDB.db3");
        ILogService _logService;
        public DataService(ILogService logService)
        {
            _logService = logService;
            CreateTables();
        }
        private bool CreateTables()
        {
            SQLiteConnection conn = null;
            try
            {
                using (conn = new SQLiteConnection(DataService.DbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create))
                {
                    conn.BeginTransaction();

                    if (!conn.TableMappings.Any(x => x.MappedType == typeof(WeatherZone)))
                    {
                        var r = conn.CreateTable<WeatherZone>();
                    }
                    if (!conn.TableMappings.Any(x => x.MappedType == typeof(Coord)))
                    {
                        var r = conn.CreateTable<Coord>();
                    }
                    if (!conn.TableMappings.Any(x => x.MappedType == typeof(Main)))
                    {
                        var r = conn.CreateTable<Main>();
                    }
                    if (!conn.TableMappings.Any(x => x.MappedType == typeof(Sys)))
                    {
                        var r = conn.CreateTable<Sys>();
                    }
                    if (!conn.TableMappings.Any(x => x.MappedType == typeof(DataModels.Weather)))
                    {
                        var r = conn.CreateTable<DataModels.Weather>();
                    }
                    if (!conn.TableMappings.Any(x => x.MappedType == typeof(Wind)))
                    {
                        var r = conn.CreateTable<Wind>();
                    }
                    if (!conn.TableMappings.Any(x => x.MappedType == typeof(Clouds)))
                    {
                        var r = conn.CreateTable<Clouds>();
                    }
                    conn.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (conn != null)
                { conn.Rollback(); }
                _logService.WriteLogAsync(ex, "CreateTables");
                return false;
            }
        }
        public  bool AddWeather(Data.Weather_Zone.WeatherZone weather, string zone)
        {
            SQLiteConnection conn = null;
            try
            {
                using (conn = new SQLiteConnection(DataService.DbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create))
                {
                    conn.BeginTransaction();
                    var dateSave = DateTime.Now.ToFileTimeUtc();

                    var wayCommandText = $"SELECT COUNT(*) FROM WeatherZones";
                    var cmd = new SQLiteCommand(conn) { CommandText = wayCommandText };
                    var id = cmd.ExecuteScalar<int>() + 1;

                    wayCommandText = $"INSERT INTO WeatherZones (id, Zone, Wbase, Visibility, Dt, Timezone, WeatherZoneId, Name, Cod, DateSave) " +
                    $" VALUES ({id},'{zone}','{weather.wbase}',{weather.visibility},{weather.dt}, {weather.timezone}, {weather.id},'{weather.name}', {weather.cod},{dateSave})";
                    cmd = new SQLiteCommand(conn) { CommandText = wayCommandText };
                    cmd.ExecuteNonQuery();

                    wayCommandText = $"INSERT INTO Clouds (WeatherZoneId, AllClouds, DateSave) " +
                        $" VALUES ({id},'{weather.clouds.all}',{dateSave})";
                    cmd = new SQLiteCommand(conn) { CommandText = wayCommandText };
                    cmd.ExecuteNonQuery();

                    wayCommandText = $"INSERT INTO Coords (WeatherZoneId, Lat, Lon,DateSave) " +
                        $" VALUES({id},{DoubleToString(weather.coord.lat)},{DoubleToString(weather.coord.lon)},{dateSave})";
                    cmd = new SQLiteCommand(conn) { CommandText = wayCommandText };
                    cmd.ExecuteNonQuery();

                    wayCommandText = $"INSERT INTO Main (WeatherZoneId, Temp,FeelLike,TempMin,TempMax,Pressure,Humidity,DateSave) " +
                        $" VALUES({id},{DoubleToString(weather.main.temp)}," +
                        $" {DoubleToString(weather.main.feels_like)},{DoubleToString(weather.main.temp_min)}" +
                        $",{DoubleToString(weather.main.temp_max)},{weather.main.pressure},{weather.main.humidity},{dateSave})";
                    cmd = new SQLiteCommand(conn) { CommandText = wayCommandText };
                    cmd.ExecuteNonQuery();

                    wayCommandText = $"INSERT INTO Sys (WeatherZoneId,Type,SysId,Message,Country,Sunrise,Sunset,DateSave) " +
                        $" VALUES({id},{weather.sys.type},{weather.sys.id},'{DoubleToString(weather.sys.message)}'" +
                        $",'{weather.sys.country}',{weather.sys.sunrise},{weather.sys.sunset},{dateSave})";
                    cmd = new SQLiteCommand(conn) { CommandText = wayCommandText };
                    cmd.ExecuteNonQuery();

                    foreach (var w in weather.weather)
                    {
                        wayCommandText = $"INSERT INTO Weather (WeatherZoneId, WeatherId,Main, Description,Icon,DateSave) " +
                            $" VALUES({id},{w.id},'{w.main}','{w.description}','{w.icon}',{dateSave})";
                        cmd = new SQLiteCommand(conn) { CommandText = wayCommandText };
                        cmd.ExecuteNonQuery();
                    }

                    wayCommandText = $"INSERT INTO Winds (WeatherZoneId,Speed,Deg,DateSave) " +
                        $" VALUES({id},{DoubleToString(weather.wind.speed)},{weather.wind.deg},{dateSave})";
                    cmd = new SQLiteCommand(conn) { CommandText = wayCommandText };
                    cmd.ExecuteNonQuery();

                    conn.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                conn.Rollback();
                _logService.WriteLog(ex, " AddWeather");
                return false;
            }
        }

        private string DoubleToString(double? d)
        {
            if (d == null) { return "0"; }
            return d?.ToString(CultureInfo.InvariantCulture);
        }

        public Data.Weather_Zone.WeatherZone GetWeather(string zone)
        {
            SQLiteConnection conn = null;
            Data.Weather_Zone.WeatherZone weatherZone = null;
            try
            {
                using (conn = new SQLiteConnection(DataService.DbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create))
                {

                    var commandText = $"SELECT wz.id, wz.Zone, wz.Wbase, wz.Visibility, wz.Dt, wz.Timezone, wz.WeatherZoneId, wz.Name, wz.Cod, wz.DateSave" +
                        $" FROM WeatherZones wz WHERE wz.DateSave = (SELECT max(DateSave) FROM WeatherZones as t2 WHERE t2.Zone = wz.Zone) AND wz.Zone = '{zone}'";
                    var cmd = new SQLiteCommand(conn) { CommandText = commandText };
                    weatherZone = cmd.ExecuteQuery<Data.Weather_Zone.WeatherZone>().FirstOrDefault();

                    commandText = $"SELECT cl.AllClouds FROM Clouds cl WHERE cl.WeatherZoneId = {weatherZone.id}";
                    cmd = new SQLiteCommand(conn) { CommandText = commandText };
                    var cloudsDb = cmd.ExecuteQuery<Data.Weather_Zone.Clouds>().FirstOrDefault();

                    commandText = $"SELECT co.Lat, co.Lon FROM Coords co WHERE co.WeatherZoneId = {weatherZone.id}";
                    cmd = new SQLiteCommand(conn) { CommandText = commandText };
                    var coordDb = cmd.ExecuteQuery<Data.Weather_Zone.Coord>().FirstOrDefault();

                    commandText = $"SELECT mn.Temp,mn.FeelLike,mn.TempMin,mn.TempMax, mn.Humidity, mn.Pressure FROM main mn WHERE mn.WeatherZoneId = {weatherZone.id}";
                    cmd = new SQLiteCommand(conn) { CommandText = commandText };
                    var mainDb = cmd.ExecuteQuery<Data.Weather_Zone.Main>().FirstOrDefault();

                    commandText = $"SELECT ss.Type,ss.SysId,ss.Message,ss.Country,ss.Sunrise,ss.Sunset FROM Sys ss WHERE ss.WeatherZoneId = {weatherZone.id}";
                    cmd = new SQLiteCommand(conn) { CommandText = commandText };
                    var sysDb = cmd.ExecuteQuery<Data.Weather_Zone.Sys>().FirstOrDefault();

                    commandText = $"SELECT ww.WeatherId, ww.main, ww.Description, ww.Icon FROM Weather ww WHERE ww.WeatherZoneId = {weatherZone.id}";
                    cmd = new SQLiteCommand(conn) { CommandText = commandText };
                    var weatherDb = cmd.ExecuteQuery<Data.Weather_Zone.Weather>();

                    commandText = $"SELECT wi.Speed, wi.Deg FROM Winds wi WHERE wi.WeatherZoneId = {weatherZone.id}";
                    cmd = new SQLiteCommand(conn) { CommandText = commandText };
                    var winDb = cmd.ExecuteQuery<Data.Weather_Zone.Wind>().FirstOrDefault();

                    weatherZone.coord = coordDb;
                    weatherZone.weather = weatherDb;
                    weatherZone.main = mainDb;
                    weatherZone.sys = sysDb;
                    weatherZone.clouds = cloudsDb;
                    weatherZone.wind = winDb;
                }
                return weatherZone;
            }
            catch (Exception ex)
            {
                _logService.WriteLog(ex, " GetWeather");
                return new Data.Weather_Zone.WeatherZone();
            }
        }
    }
}
