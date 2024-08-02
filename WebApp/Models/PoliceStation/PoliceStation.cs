using System.Collections.Generic;
using WebApp.Models.Shared;

namespace WebApp.Models.PoliceStation;

public class PoliceStation
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public Location Location { get; set; }
    public List<string> Services { get; set; }


    public override string ToString()
    {
        return $"{Id} {Name} {Url} {Location} {Services.ToArray()}";
    }

}
