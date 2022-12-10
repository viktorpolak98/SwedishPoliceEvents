using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
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
