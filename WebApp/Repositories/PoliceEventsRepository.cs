using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Models.PoliceEvent;
using WebApp.Models.Shared;
using System.Text.Json;
using WebApp.HelperFunctions;
using System.Globalization;
using System.Threading.Tasks;
using WebApp.Logic;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;

namespace WebApp.Repositories
{
    /// <summary>
    /// Repository used to store data regarding PoliceEvents. Inherits PoliceAPICaller class.
    /// </summary>
    public class PoliceEventsRepository : PoliceAPICaller, IRepository<PoliceEvent, EventType>
    {
        private readonly List<PoliceEvent> Events = new List<PoliceEvent>();
        private readonly Dictionary<string, EventType> EventTypeDict;
        private readonly Leaderboard leaderboard;
        private readonly MemoryCache MemCache = new MemoryCache(new MemoryCacheOptions());

        private readonly SemaphoreSlim RequestSemaphore = new SemaphoreSlim(1,1);

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

        /// <summary>
        /// Creates PoliceEvents with API call from PoliceAPICaller class
        /// Utilizes caching to limit the amount of api calls. Uses a Semaphore to limit 1 thread to call API at a time
        /// </summary>
        /// <param name="path">Path to api to call</param>
        /// <returns></returns>
        public async Task CreateValues(string path)
        {

            //If Memcache.Count == 500 data about PoliceEvents already exists and there is no need to do a api call
            if (AmountOfCachedItems() == 500)
            {
                return;
            }

            RequestSemaphore.Wait();

            //Recheck if another thread has done the API call
            if (AmountOfCachedItems() == 500)
            {
                RequestSemaphore.Release();
                return;
            }

            await CreateEvents(path);

            RequestSemaphore.Release();
        }

        /// <summary>
        /// Clears dictionaries and list of events before new ones are created
        /// </summary>
        private void BeforeCreateEvents()
        {
            Events.Clear();
            leaderboard.ClearDictionaries();

        }

        /// <summary>
        /// Sorts leaderboard dictionaries after events are created
        /// </summary>
        private void AfterCreateEvents()
        {
            leaderboard.SortDictionaries(true);
        }

        /// <summary>
        /// Creates events from a jsondocument and populates the cache with created events. Exists in the cache for 10 minutes
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task CreateEvents(string path)
        {
            JsonDocument doc = await ReadData(path);

            if (doc == null)
            {
                return;
            }
            BeforeCreateEvents();

            foreach (JsonElement Element in doc.RootElement.EnumerateArray())
            {

                string[] gps = Element.GetProperty("location").GetProperty("gps").ToString().Split(",");
                string locationName = Element.GetProperty("location").GetProperty("name").ToString();
                EventType eventType = GetType(Element.GetProperty("type").ToString());

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


                using (var entry = MemCache.CreateEntry(Event.Id))
                {
                    entry.Value = Event;
                    entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(10);
                }

                leaderboard.AddCountEvent(eventType);
                leaderboard.AddCountEventLocation(locationName);
            }

            AfterCreateEvents();

            doc.Dispose();

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

            return Events.Where(E => E.Location.Name == locationName).ToList();
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
        public List<PoliceEvent> GetAllByType(EventType type)
        {
            return Events.Where(E => E.Type == type).ToList();
        }


        /// <summary>
        /// Returns all PoliceEvents from a specific EventType display name
        /// </summary>
        /// <param name="displayName">Display name to return</param>
        /// <returns></returns>
        public List<PoliceEvent> GetAllByDisplayName(string displayName)
        {
            if (!EventTypeDict.ContainsKey(displayName))
            {
                return new List<PoliceEvent>();
            }

            return Events.Where(E => E.Type == EventTypeDict[displayName]).ToList();
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
        /// Returns an EventType with display name as key
        /// </summary>
        /// <param name="key">Display name key</param>
        /// <returns></returns>
        public EventType GetType(string key)
        {
            EventTypeDict.TryGetValue(key, out EventType eventType);

            return eventType;
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

        /// <summary>
        /// Returns amount of cached events
        /// Used for testing if several threads access CreateValues() simultaneously
        /// </summary>
        /// <returns></returns>
        public int AmountOfCachedItems()
        {
            return MemCache.Count;
        }

        /// <summary>
        /// Validates that the current Events exists in MemCache
        /// </summary>
        /// <returns></returns>
        public List<PoliceEvent> ValidateEntries()
        {
            List<PoliceEvent> listEvents = new List<PoliceEvent>();
            foreach(var val in Events)
            {
                MemCache.TryGetValue(val.Id, out PoliceEvent pEvent);
                listEvents.Add(pEvent);
            }
            return listEvents;
        }
    }
}
