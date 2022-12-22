namespace WebApp.Models.Shared
{
    /// <summary>
    /// Model class for location used by both PoliceEvent and PoliceStation
    /// </summary>
    public class Location
    {
        public string Name { get; set; }
        public GPSLocation GpsLocation { get; set; }

        public override string ToString()
        {
            return $"{Name} {GpsLocation}";
        }
    }
}
