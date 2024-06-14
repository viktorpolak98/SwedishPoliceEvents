using System.Collections.Generic;
using System.Linq;
using WebApp.Models.PoliceStation;
using WebApp.Models.Shared;
using System.Text.Json;
using WebApp.HelperFunctions;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using System;
using WebApp.Logic;

namespace WebApp.Repositories
{
    /// <summary>
    /// Repository used to store data regarding PoliceStations.
    /// WORK IN PROGRESS
    /// </summary>
    public class PoliceStationsRepository : IRepository<PoliceStation, ServiceType>
    {
        private readonly List<PoliceStation> Stations = [];
        private readonly Dictionary<string, ServiceType> ServiceTypeDict;
        private readonly SemaphoreSlim RequestSemaphore = new (1, 1);
        private readonly MemoryCache MemCache = new (new MemoryCacheOptions());
        public Leaderboard<ServiceType> leaderboard { get; }
        private int NumberOfStations { get; set; } = -1; //-1 = no value exists

        /// <summary>
        /// For testing purposes
        /// </summary>
        /// <param name="Events"> A set list of PoliceStations </param>
        public PoliceStationsRepository(List<PoliceStation> Stations)
        {
            this.Stations = Stations;
            ServiceTypeDict = EnumValuesHelper.ToDictionaryDisplayNameAsKey<ServiceType>();
            leaderboard = new Leaderboard<ServiceType>();
        }

        public PoliceStationsRepository()
        {
            ServiceTypeDict = EnumValuesHelper.ToDictionaryDisplayNameAsKey<ServiceType>();
            leaderboard = new Leaderboard<ServiceType>();
        }

        /// <summary>
        /// Creates PoliceStations with API call from PoliceAPICaller class
        /// Utilizes caching to limit the amount of api calls. Uses a Semaphore to limit 1 thread to call API at a time
        /// </summary>
        /// <param name="path">Path to api to call</param>
        /// <returns></returns>
        public void CreateValues(JsonDocument doc)
        {
            //If Memcache.Count == NumberOfStations data about PoliceStations already exists and there is no need to do a api call MemCache keeps data for 24hours
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

            CreateStations(doc);

            RequestSemaphore.Release();
        }

        public void CreateStations(JsonDocument doc)
        {

            if (doc is null)
            {
                return;
            }
            Stations.Clear();

            foreach (JsonElement Element in doc.RootElement.EnumerateArray())
            {
                List<ServiceType> serviceTypes = new();
                foreach(JsonElement service in Element.GetProperty("services").EnumerateArray())
                {
                    serviceTypes.Add(GetType(service.GetProperty("name").ToString()));
                }
                
                string[] gps = Element.GetProperty("location").GetProperty("gps").ToString().Split(",");


                PoliceStation station = new ()
                {
                    Id = Element.GetProperty("id").ToString(),
                    Name = Element.GetProperty("name").ToString(),
                    Url = Element.GetProperty("Url").ToString(),
                    Location = new Location
                    {
                        Name = Element.GetProperty("location").GetProperty("name").ToString(),
                        GpsLocation = new GPSLocation
                        {
                            Latitude = gps[0],
                            Longitude = gps[1]
                        }
                    },
                    Services = serviceTypes
                };

                Stations.Add(station);
                CreateCacheEntry(station);

            }

            NumberOfStations = Stations.Count;
            doc.Dispose();

        }

        public void CreateCacheEntry(PoliceStation key, int time = 1)
        {
            using var entry = MemCache.CreateEntry(key.Id);
            entry.Value = key;
            entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(time);
        }

        /// <summary>
        /// Gets a PoliceStation from latitude and logitude
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lon">longitude</param>
        /// <returns></returns>
        public List<PoliceStation> GetAllByLatLon(string lat, string lon)
        {
            return Stations.Where(E => E.Location.GpsLocation.Latitude.Equals(lat)
                                    && E.Location.GpsLocation.Longitude.Equals(lon)).ToList();
        }

        /// <summary>
        /// Gets all PoliceStations from a location name
        /// </summary>
        /// <param name="locationName">Location name</param>
        /// <returns></returns>
        public List<PoliceStation> GetAllByLocationName(string locationName)
        {

            return Stations.Where(E => E.Location.Name == locationName).ToList();
        }

        /// <summary>
        /// Gets a specific PoliceStation based on an id
        /// </summary>
        /// <param name="id">Id for PoliceStation</param>
        /// <returns></returns>
        public PoliceStation GetById(string id)
        {
            return Stations.Where(E => E.Id == id).SingleOrDefault();
        }

        /// <summary>
        /// Returns all PoliceStations who offer the specified service
        /// </summary>
        /// <param name="type">Specific service</param>
        /// <returns></returns>
        public List<PoliceStation> GetAllByType(ServiceType type)
        {
            return Stations.Where(E => E.Services.Contains(type)).ToList();
        }

        /// <summary>
        /// Returns all PoliceStations who offer the specified service based on display name
        /// </summary>
        /// <param name="displayName">Service as display name</param>
        /// <returns></returns>
        public List<PoliceStation> GetAllByDisplayName(string displayName)
        {
            if (!ServiceTypeDict.ContainsKey(displayName))
            {
                return [];
            }

            return Stations.Where(E => E.Services.Contains(ServiceTypeDict[displayName])).ToList();
        }

        /// <summary>
        /// Gets a ServiceType from specified key
        /// </summary>
        /// <param name="key">Key to ServiceType</param>
        /// <returns></returns>
        public ServiceType GetType(string key)
        {
            ServiceTypeDict.TryGetValue(key, out ServiceType serviceType);

            return serviceType;
        }

        /// <summary>
        /// Returns the amount of PoliceStations
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return Stations.Count;
        }

        public bool CacheIsFull()
        {
            return NumberOfStations == MemCache.Count;
        }

        /// <summary>
        /// Returns the PoliceStations list
        /// </summary>
        /// <returns></returns>
        public List<PoliceStation> GetAll()
        {
            return Stations;
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
        /// Validates that the current stations exists in MemCache
        /// </summary>
        /// <returns></returns>
        public List<PoliceStation> ValidateEntries()
        {
            List<PoliceStation> listStations = new();
            foreach (var val in Stations)
            {
                MemCache.TryGetValue(val.Id, out PoliceStation pStation);
                listStations.Add(pStation);
            }
            return listStations;
        }

        public Dictionary<ServiceType, int> GetTypeLeaderboard()
        {
            return leaderboard.NumberOfTypeDict;
        }

        public Dictionary<string, int> GetLocationLeaderboard()
        {
            return leaderboard.NumberOfLocationDict;
        }
    }
}
