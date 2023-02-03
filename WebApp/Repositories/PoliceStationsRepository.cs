using System.Collections.Generic;
using System.Linq;
using WebApp.Models.PoliceStation;
using WebApp.Models.Shared;
using System.Text.Json;
using WebApp.HelperFunctions;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using WebApp.Models.PoliceEvent;

namespace WebApp.Repositories
{
    /// <summary>
    /// Repository used to store data regarding PoliceStations. Inherits PoliceAPICaller class.
    /// WORK IN PROGRESS
    /// </summary>
    public class PoliceStationsRepository : PoliceAPICaller, IRepository<PoliceStation, ServiceType>
    {
        private readonly List<PoliceStation> Stations = new List<PoliceStation>();
        private readonly Dictionary<string, ServiceType> ServiceTypeDict;
        private readonly SemaphoreSlim RequestSemaphore = new SemaphoreSlim(1, 1);
        private readonly MemoryCache MemCache = new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// For testing purposes
        /// </summary>
        /// <param name="Events"> A set list of PoliceStations </param>
        public PoliceStationsRepository(List<PoliceStation> Stations)
        {
            this.Stations = Stations;
            ServiceTypeDict = EnumValuesHelper.ToDictionaryDisplayNameAsKey<ServiceType>();
        }

        public PoliceStationsRepository()
        {
            ServiceTypeDict = EnumValuesHelper.ToDictionaryDisplayNameAsKey<ServiceType>();
        }

        /// <summary>
        /// Creates PoliceStations with API call from PoliceAPICaller class
        /// Utilizes caching to limit the amount of api calls. Uses a Semaphore to limit 1 thread to call API at a time
        /// </summary>
        /// <param name="path">Path to api to call</param>
        /// <returns></returns>
        public async Task CreateValues(string path)
        {
            //If Memcache.Count == 500 data about PoliceEvents already exists and there is no need to do a api call
            if (MemCache.Count == 500)
            {
                return;
            }

            RequestSemaphore.Wait();

            //Recheck if another thread has done the API call
            if (MemCache.Count == 500)
            {
                RequestSemaphore.Release();
                return;
            }

            await CreateStations(path);

            RequestSemaphore.Release();
        }

        public async Task CreateStations(string path)
        {
            JsonDocument doc = await ReadData(path);

            if (doc == null) return;

            Stations.Clear();


            foreach (JsonElement Element in doc.RootElement.EnumerateArray())
            {

                List<ServiceType> serviceTypes = new List<ServiceType>();
                foreach(JsonElement service in Element.GetProperty("services").EnumerateArray())
                {
                    serviceTypes.Add(GetType(service.GetProperty("name").ToString()));
                }
                
                string[] gps = Element.GetProperty("location").GetProperty("gps").ToString().Split(",");


                PoliceStation station = new PoliceStation
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

                using var entry = MemCache.CreateEntry(station.Id);
                entry.Value = station;
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(1);

            }


            doc.Dispose();

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
                return new List<PoliceStation>();
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
            List<PoliceStation> listStations = new List<PoliceStation>();
            foreach (var val in Stations)
            {
                MemCache.TryGetValue(val.Id, out PoliceStation pStation);
                listStations.Add(pStation);
            }
            return listStations;
        }
    }
}
