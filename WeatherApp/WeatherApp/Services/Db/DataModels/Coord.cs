using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WeatherApp.Services.Db.DataModels
{
    [Table("Coords")]
    public class Coord
    {
        [PrimaryKey, AutoIncrement]
        public long? Id { get; set; }
        [ForeignKey(typeof(WeatherZone))]
        public long WeatherZoneId { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public long DateSave { get; set; }
    }
}
