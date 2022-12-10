using System;

namespace WebApp.Models
{
    public class PoliceEvent
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
        public EventType Type { get; set; }
        public Location Location { get; set; }

        
        public override string ToString()
        {
            return $"{Id} {Date} {Name} {Summary} {Url} {Type} {Location}";
        }
    }
}
