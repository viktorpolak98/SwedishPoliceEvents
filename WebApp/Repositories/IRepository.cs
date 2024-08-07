﻿using System.Collections.Generic;
using System.Text.Json;

namespace WebApp.Repositories;

public interface IRepository<T> where T : class
{
    public List<T> GetAll();

    public T GetById(string id);

    public List<T> GetAllByType(string type);

    public List<T> GetAllByLatLon(string lat, string lon);

    public List<T> GetAllByLocationName(string locationName);

    public int GetCount();

    public void CreateValues(JsonDocument values);

    public void CreateCacheEntry(T key, int time);

    public bool CacheIsFull();
}
