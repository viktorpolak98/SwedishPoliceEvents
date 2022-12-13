using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Models;
using System.Text.Json;
using WebApp.HelperFunctions;
using System.Globalization;
using System.Threading.Tasks;
using WebApp.Logic;

namespace WebApp.Repositories
{
    public class PoliceEventsRepository : PoliceAPICaller
    {
        private readonly List<PoliceEvent> Events = new List<PoliceEvent>();
        private readonly Dictionary<string, EventType> EventTypeDict;
        private readonly Leaderboard leaderboard; 

        /// <summary>
        /// For testing purposes
        /// </summary>
        /// <param name="Events"> A set list of PoliceEvents </param>
        public PoliceEventsRepository(List<PoliceEvent> Events)
        {
            this.Events = Events;
            EventTypeDict = EnumValuesHelper.ToDictionaryDisplayNameAsKey<EventType>();
            leaderboard = new Leaderboard();
        }


        public PoliceEventsRepository()
        {
            EventTypeDict = EnumValuesHelper.ToDictionaryDisplayNameAsKey<EventType>();
            leaderboard = new Leaderboard();
        }

        public async Task CreateEvents(string path)
        {
            JsonDocument doc = await ReadData(path);

            if (doc == null) return;

            Events.Clear();
            leaderboard.ClearDictionaries();

            foreach (JsonElement Element in doc.RootElement.EnumerateArray())
            {

                string[] gps = Element.GetProperty("location").GetProperty("gps").ToString().Split(",");
                string locationName = Element.GetProperty("location").GetProperty("name").ToString();
                EventType eventType = GetEventType(Element.GetProperty("type").ToString());

                Events.Add(new PoliceEvent
                {
                    Id = Element.GetProperty("id").ToString(),
                    Date = DateConverter(Element.GetProperty("datetime").ToString()),
                    Name = Element.GetProperty("name").ToString(),
                    Summary = Element.GetProperty("summary").ToString(),
                    Url = Element.GetProperty("url").ToString(),
                    Type = eventType,
                    Location = new Location
                    {
                        Name = locationName,
                        GpsLocation = new GPSLocation
                        {
                            Latitude = gps[0],
                            Longitude = gps[1]
                        }
                    }
                });

                leaderboard.AddCountEvent(eventType);
                leaderboard.AddCountEventLocation(locationName);
            }


            doc.Dispose();

        }

        public List<PoliceEvent> GetPoliceEventsFromLatLon(string lat, string lon)
        {
            return Events.Where(E => E.Location.GpsLocation.Latitude.Equals(lat)
                                    && E.Location.GpsLocation.Longitude.Equals(lon)).ToList();
        }

        public List<PoliceEvent> GetPoliceEventsFromLocationName(string locationName)
        {

            return Events.Where(E => E.Location.Name == locationName).ToList();
        }

        public PoliceEvent GetPoliceEventFromId(string id)
        {
            return Events.Where(E => E.Id == id).SingleOrDefault();
        }

        public List<PoliceEvent> GetPoliceEventsFromType(EventType type)
        {
            return Events.Where(E => E.Type == type).ToList();
        }

        public List<PoliceEvent> GetPoliceEventsFromTypeDisplayName(string displayName)
        {
            if (!EventTypeDict.ContainsKey(displayName))
            {
                return new List<PoliceEvent>();
            }

            return Events.Where(E => E.Type == EventTypeDict[displayName]).ToList();
        }

        public DateTime DateConverter(string date)
        {
            DateTime.TryParseExact(date, "yyyy-MM-dd H:mm:ss zzz", CultureInfo.InvariantCulture,
                         DateTimeStyles.None, out DateTime dateTime);

            return dateTime;
        }

        public EventType GetEventType(string key)
        {
            EventTypeDict.TryGetValue(key, out EventType eventType);

            return eventType;
        }

        public int GetNumberOfPoliceEvents()
        {
            return Events.Count;
        }

        public List<PoliceEvent> GetAllEvents()
        {
            return Events;
        }
    }
}
