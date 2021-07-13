using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WeatherApp.Services.Db.DataModels
{
    [Table("Sys")]
    public class Sys
    {
        [PrimaryKey, AutoIncrement]
        public long? Id { get; set; }
        [ForeignKey(typeof(WeatherZone))]
        public long WeatherZoneId { get; set; }
        public int Type { get; set; }
        public int SysId { get; set; }
        public double Message { get; set; }
        public string Country { get; set; }
        public int Sunrise { get; set; }
        public int Sunset { get; set; }
        public long DateSave { get; set; }
    }
}
