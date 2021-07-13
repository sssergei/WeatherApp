using SQLite;

namespace WeatherApp.Services.Db.DataModels
{
    [Table("WeatherZones")]
    public class WeatherZone
    {
        [PrimaryKey]
        public long id { get; set; }
        public string Zone { get; set; }
        public string Wbase { get; set; }
        public int Visibility { get; set; }
        public int Dt { get; set; }
        public int Timezone { get; set; }
        public int WeatherZoneId { get; set; }
        public string Name { get; set; }
        public int Cod { get; set; }

        //[OneToMany(CascadeOperations = CascadeOperation.All)]
        //public List<Coord> Coords { get; set; }
        public long DateSave { get; set; }
    }
}
