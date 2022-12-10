using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
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
