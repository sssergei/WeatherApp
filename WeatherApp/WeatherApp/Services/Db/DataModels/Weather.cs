using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WeatherApp.Services.Db.DataModels
{
    [Table("Weather")]
    public class Weather
    {
        [PrimaryKey, AutoIncrement]
        public long? Id { get; set; }
        [ForeignKey(typeof(WeatherZone))]
        public long WeatherZoneId { get; set; }
        public int WeatherId { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public long DateSave { get; set; }
    }
}
