namespace WebApp.Models.Shared;

public class GPSLocation
{
    public string Latitude { get; set; }
    public string Longitude { get; set; }

    public override string ToString()
    {
        return $"{Latitude},{Longitude}";
    }
}
