using System.Collections.Generic;
using WebApp.Models.Shared;

namespace WebApp.Models.PoliceStation
{
    /// <summary>
    /// Model class for a police event
    /// </summary>
    public class PoliceStation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Location Location { get; set; }
        public List<ServiceType> Services { get; set; }


        public override string ToString()
        {
            return $"{Id} {Name} {Url} {Location} {Services.ToArray()}";
        }

    }

}
