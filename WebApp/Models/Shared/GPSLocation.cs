namespace WebApp.Models.Shared;

public class GPSLocation
{
    public string Longitude { get; set; }
    public string Latitude { get; set; }

    public override string ToString()
    {
        return $"{Latitude},{Longitude}";
    }
}
