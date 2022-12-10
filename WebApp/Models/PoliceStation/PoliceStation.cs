using System;
using System.Collections.Generic;

namespace WebApp.Models
{
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
