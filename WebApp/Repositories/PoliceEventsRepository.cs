using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Models.PoliceEvent;
using WebApp.Models.Shared;
using System.Text.Json;
using System.Globalization;
using WebApp.Logic;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;

namespace WebApp.Repositories
{
    /// <summary>
    /// Repository used to store data regarding PoliceEvents. Inherits PoliceAPICaller class.
    /// </summary>
    public class PoliceEventsRepository : IRepository<PoliceEvent>
    {
        private readonly List<PoliceEvent> Events = [];
        public Leaderboard Leaderboard { get; }
        private readonly MemoryCache MemCache = new(new MemoryCacheOptions());

        private readonly SemaphoreSlim RequestSemaphore = new(1,1);

        /// <summary>
        /// For testing purposes
        /// </summary>
        /// <param name="Events"> A set list of PoliceEvents </param>
        public PoliceEventsRepository(List<PoliceEvent> Events)
        {
            this.Events = Events;
            Leaderboard = new Leaderboard();
        }


        public PoliceEventsRepository()
        {
            Leaderboard = new Leaderboard();
        }

        /// <summary>
        /// Creates PoliceEvents with API call from PoliceAPICaller class
        /// Utilizes caching to limit the amount of api calls. Uses a Semaphore to limit 1 thread to call API at a time
        /// </summary>
        /// <param name="path">Path to api to call</param>
        /// <returns></returns>
        public void CreateValues(JsonDocument events)
        {

            //If Memcache.Count == 500 data about PoliceEvents already exists and there is no need to do a api call
            if (CacheIsFull())
            {
                return;
            }

            RequestSemaphore.Wait();

            //Recheck if another thread has done the API call
            if (CacheIsFull())
            {
                RequestSemaphore.Release();
                return;
            }

            CreateEvents(events);

            RequestSemaphore.Release();
        }

        /// <summary>
        /// Clears dictionaries and list of events before new ones are created
        /// </summary>
        private void BeforeCreateEvents()
        {
            Events.Clear();
            Leaderboard.ClearDictionaries();

        }

        /// <summary>
        /// Creates events from a jsondocument and populates the cache with created events. Exists in the cache for 10 minutes
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private void CreateEvents(JsonDocument events)
        {

            if (events is null)
            {
                return;
            }
            BeforeCreateEvents();

            foreach (JsonElement Element in events.RootElement.EnumerateArray())
            {

                string[] gps = Element.GetProperty("location").GetProperty("gps").ToString().Split(",");
                string locationName = Element.GetProperty("location").GetProperty("name").ToString();
                string eventType = Element.GetProperty("type").ToString();

                PoliceEvent Event = new ()
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
                };

                Events.Add(Event);
                CreateCacheEntry(Event);

                Leaderboard.AddCountEvent(eventType);
                Leaderboard.AddCountEventLocation(locationName);
            }

            events.Dispose();

        }

        public void CreateCacheEntry(PoliceEvent key, int time = 10)
        {
            using var entry = MemCache.CreateEntry(key.Id);
            entry.Value = key;
            entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(time);
        }

        /// <summary>
        /// Gets all PoliceEvents from specified latitude and longitude
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lon">Longitude</param>
        /// <returns></returns>
        public List<PoliceEvent> GetAllByLatLon(string lat, string lon)
        {
            return Events.Where(E => E.Location.GpsLocation.Latitude.Equals(lat)
                                    && E.Location.GpsLocation.Longitude.Equals(lon)).ToList();
        }

        /// <summary>
        /// Gets aall PoliceEvents from specified location name
        /// </summary>
        /// <param name="locationName">Name of location</param>
        /// <returns></returns>
        public List<PoliceEvent> GetAllByLocationName(string locationName)
        {
            return Events.Where(E => E.Location.Name.Equals(locationName)).ToList();
            
        }

        /// <summary>
        /// Returns a specific PoliceEvent with the specified id
        /// If id does not exist, null is returned 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PoliceEvent GetById(string id)
        {
            return Events.Where(E => E.Id == id).SingleOrDefault();
        }

        /// <summary>
        /// Returns all PoliceEvents from a specific EventType
        /// </summary>
        /// <param name="type">EventType to return</param>
        /// <returns></returns>
        public List<PoliceEvent> GetAllByType(string type)
        {
            return Events.Where(E => E.Type == type).ToList();
        }

        /// <summary>
        /// Converts the date returned from api to a DateTime. Uses local culture
        /// </summary>
        /// <param name="date">Date to convert as string</param>
        /// <returns></returns>
        public DateTime DateConverter(string date)
        {
            DateTime.TryParseExact(date, "yyyy-MM-dd H:mm:ss zzz", CultureInfo.InvariantCulture,
                         DateTimeStyles.None, out DateTime dateTime);

            return dateTime;
        }


        /// <summary>
        /// Gets the amount of PoliceEvents in events list
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return Events.Count;
        }

        /// <summary>
        /// Returns the PoliceEvents list
        /// </summary>
        /// <returns></returns>
        public List<PoliceEvent> GetAll()
        {
            return Events;
        }

        public bool CacheIsFull()
        {
            return MemCache.Count == 500;
        }

        /// <summary>
        /// Validates that the current Events exists in MemCache
        /// </summary>
        /// <returns></returns>
        public List<PoliceEvent> ValidateEntries()
        {
            List<PoliceEvent> listEvents = [];
            foreach(var val in Events)
            {
                MemCache.TryGetValue(val.Id, out PoliceEvent pEvent);
                listEvents.Add(pEvent);
            }

            return listEvents;
        }

        public Dictionary<string, int> GetTypeLeaderboard()
        {
            return Leaderboard.NumberOfTypeDict;
        }

        public Dictionary<string, int> GetLocationLeaderboard()
        {
            return Leaderboard.NumberOfLocationDict;
        }
    }
}
