using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Models;
using System.Text.Json;
using WebApp.HelperFunctions;
using System.Threading.Tasks;

namespace WebApp.Repositories
{
    public class PoliceStationsRepository : PoliceAPICaller
    {
        private readonly List<PoliceStation> Stations = new List<PoliceStation>();
        private readonly Dictionary<string, ServiceType> ServiceTypeDict;

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
                    serviceTypes.Add(GetServiceType(service.GetProperty("name").ToString()));
                }
                
                string[] gps = Element.GetProperty("location").GetProperty("gps").ToString().Split(",");


                Stations.Add(new PoliceStation
                {
                    Id = Element.GetProperty("id").ToString(),
                    Name = Element.GetProperty("name").ToString(),
                    Url = Element.GetProperty("url").ToString(),
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
                });

            }


            doc.Dispose();

        }

        public List<PoliceStation> GetPoliceStationFromLatLon(string lat, string lon)
        {
            return Stations.Where(E => E.Location.GpsLocation.Latitude.Equals(lat)
                                    && E.Location.GpsLocation.Longitude.Equals(lon)).ToList();
        }

        public List<PoliceStation> GetPoliceStationFromLocationName(string locationName)
        {

            return Stations.Where(E => E.Location.Name == locationName).ToList();
        }

        public PoliceStation GetPoliceStationFromId(string id)
        {
            return Stations.Where(E => E.Id == id).SingleOrDefault();
        }

        public List<PoliceStation> GetServicesFromType(ServiceType type)
        {
            return Stations.Where(E => E.Services.Contains(type)).ToList();
        }

        public List<PoliceStation> GetServicesFromTypeDisplayName(string displayName)
        {
            if (!ServiceTypeDict.ContainsKey(displayName))
            {
                return new List<PoliceStation>();
            }

            return Stations.Where(E => E.Services.Contains(ServiceTypeDict[displayName])).ToList();
        }

        public ServiceType GetServiceType(string key)
        {
            ServiceTypeDict.TryGetValue(key, out ServiceType serviceType);

            return serviceType;
        }

        public int GetNumberOfPoliceStations()
        {
            return Stations.Count;
        }

        public List<PoliceStation> GetAllStations()
        {
            return Stations;
        }
    }
}
