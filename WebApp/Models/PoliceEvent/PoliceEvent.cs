using System;
using WebApp.Models.Shared;

namespace WebApp.Models.PoliceEvent
{
    /// <summary>
    /// Model class from a police event
    /// </summary>
    public class PoliceEvent
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public Location Location { get; set; }

        
        public override string ToString()
        {
            return $"{Id} {Date} {Name} {Summary} {Url} {Type} {Location}";
        }
    }
}
