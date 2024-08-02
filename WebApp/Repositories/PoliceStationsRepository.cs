using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using WebApp.Models.PoliceStation;
using WebApp.Models.Shared;

namespace WebApp.Repositories;

public class PoliceStationsRepository : IRepository<PoliceStation>
{
    private readonly List<PoliceStation> Stations = [];
    private readonly SemaphoreSlim RequestSemaphore = new(1, 1);
    private readonly MemoryCache MemCache = new(new MemoryCacheOptions());
    private int NumberOfStations { get; set; } = -1; //-1 = no value exists

    public PoliceStationsRepository(List<PoliceStation> Stations)
    {
        this.Stations = Stations;
    }

    public PoliceStationsRepository()
    {
        //Empty constructor
    }

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

        //Prevent deadlock
        try
        {
            CreateStations(doc);
        }
        finally
        {
            RequestSemaphore.Release();
        }
    }

    private void CreateStations(JsonDocument doc)
    {

        if (doc is null)
        {
            return;
        }
        Stations.Clear();

        foreach (JsonElement Element in doc.RootElement.EnumerateArray())
        {
            List<string> serviceTypes = [];
            foreach (JsonElement service in Element.GetProperty("services").EnumerateArray())
            {
                serviceTypes.Add(service.GetProperty("name").ToString());
            }

            string[] gps = Element.GetProperty("location").GetProperty("gps").ToString().Split(",");


            PoliceStation station = new()
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

    public List<PoliceStation> GetAllByLatLon(string lat, string lon)
    {
        return Stations.Where(E => E.Location.GpsLocation.Latitude.Equals(lat)
                                && E.Location.GpsLocation.Longitude.Equals(lon)).ToList();
    }

    public List<PoliceStation> GetAllByLocationName(string locationName)
    {

        return Stations.Where(E => E.Location.Name == locationName).ToList();
    }

    public PoliceStation GetById(string id)
    {
        return Stations.Where(E => E.Id == id).SingleOrDefault();
    }

    public List<PoliceStation> GetAllByType(string type)
    {
        return Stations.Where(E => E.Services.Contains(type)).ToList();
    }

    public int GetCount()
    {
        return Stations.Count;
    }

    public bool CacheIsFull()
    {
        return NumberOfStations == MemCache.Count;
    }

    public List<PoliceStation> GetAll()
    {
        return Stations;
    }

    public int AmountOfCachedItems()
    {
        return MemCache.Count;
    }

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
}
