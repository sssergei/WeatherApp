using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WeatherApp.Services.Db.DataModels
{
    [Table("Winds")]
    public class Wind
    {
        [PrimaryKey, AutoIncrement]
        public long? Id { get; set; }
        [ForeignKey(typeof(WeatherZone))]
        public long WeatherZoneId { get; set; }
        public double Speed { get; set; }
        public int Deg { get; set; }
        public long DateSave { get; set; }
    }
}
