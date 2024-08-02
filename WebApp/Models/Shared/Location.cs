namespace WebApp.Models.Shared;

public class Location
{
    public string Name { get; set; }
    public GPSLocation GpsLocation { get; set; }

    public override string ToString()
    {
        return $"{Name} {GpsLocation}";
    }
}
