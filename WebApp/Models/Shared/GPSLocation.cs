
namespace WebApp.Models.Shared
{
    /// <summary>
    /// Model class for GPS location used by both PoliceEvent and PoliceStation
    /// </summary>
    public class GPSLocation
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public override string ToString()
        {
            return $"{Longitude},{Latitude}";
        }
    }
}
