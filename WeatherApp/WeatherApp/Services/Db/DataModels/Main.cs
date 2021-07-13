using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WeatherApp.Services.Db.DataModels
{
    [Table("Main")]
    public class Main
    {
        [PrimaryKey, AutoIncrement]
        public long? Id { get; set; }
        [ForeignKey(typeof(WeatherZone))]
        public long WeatherZoneId { get; set; }
        public double Temp { get; set; }
        public double FeelLike { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public long DateSave { get; set; }
    }
}
