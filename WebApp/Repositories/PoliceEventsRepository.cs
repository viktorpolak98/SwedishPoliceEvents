using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading;
using WebApp.Models.PoliceEvent;
using WebApp.Models.Shared;

namespace WebApp.Repositories;

public class PoliceEventsRepository : IRepository<PoliceEvent>
{
    private readonly List<PoliceEvent> Events = [];
    private readonly MemoryCache MemCache = new(new MemoryCacheOptions());

    private readonly SemaphoreSlim RequestSemaphore = new(1, 1);
    private const string BaseUrl = "https://polisen.se";

    public PoliceEventsRepository(List<PoliceEvent> Events)
    {
        this.Events = Events;
    }


    public PoliceEventsRepository()
    {
        //Empty constructor
    }

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

        //Prevent deadlock
        try
        {
            CreateEvents(events);
        }
        finally
        {
            RequestSemaphore.Release();
        }
    }

    private void BeforeCreateEvents()
    {
        Events.Clear();

    }

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

            PoliceEvent Event = new()
            {
                Id = Element.GetProperty("id").ToString(),
                Date = DateConverter(Element.GetProperty("datetime").ToString()),
                Name = Element.GetProperty("name").ToString(),
                Summary = Element.GetProperty("summary").ToString(),
                Url = $"{BaseUrl}{Element.GetProperty("url")}",
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
        }

        events.Dispose();

    }

    public void CreateCacheEntry(PoliceEvent key, int time = 10)
    {
        using var entry = MemCache.CreateEntry(key.Id);
        entry.Value = key;
        entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(time);
    }

    public List<PoliceEvent> GetAllByLatLon(string lat, string lon)
    {
        return Events.Where(E => E.Location.GpsLocation.Latitude.Equals(lat)
                                && E.Location.GpsLocation.Longitude.Equals(lon)).ToList();
    }

    public List<PoliceEvent> GetAllByLocationName(string locationName)
    {
        return Events.Where(E => E.Location.Name.Equals(locationName)).ToList();

    }

    public PoliceEvent GetById(string id)
    {
        return Events.Where(E => E.Id == id).SingleOrDefault();
    }

    public List<PoliceEvent> GetAllByType(string type)
    {
        return Events.Where(E => E.Type == type).ToList();
    }

    public DateTime DateConverter(string date)
    {
        DateTime.TryParseExact(date, "yyyy-MM-dd H:mm:ss zzz", CultureInfo.InvariantCulture,
                     DateTimeStyles.None, out DateTime dateTime);

        return dateTime;
    }

    public int GetCount()
    {
        return Events.Count;
    }

    public List<PoliceEvent> GetAll()
    {
        return Events;
    }

    public bool CacheIsFull()
    {
        return MemCache.Count == 500;
    }

    public List<PoliceEvent> ValidateEntries()
    {
        List<PoliceEvent> listEvents = [];
        foreach (var val in Events)
        {
            MemCache.TryGetValue(val.Id, out PoliceEvent pEvent);
            listEvents.Add(pEvent);
        }

        return listEvents;
    }
}
