using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApp.Repositories
{
    public interface IRepository<T, TEnum> where T : class
    {
        public List<T> GetAll();

        public T GetById(string id);

        public List<T> GetAllByType(TEnum type);

        public List<T> GetAllByLatLon(string lat, string lon);

        public List<T> GetAllByLocationName(string locationName);

        public List<T> GetAllByDisplayName(string displayName);

        public TEnum GetType(string key);

        public int GetCount();

        public void CreateValues(JsonDocument values);

        public void CreateCacheEntry(T key, int time);

        public bool CacheIsFull();

        public Dictionary<TEnum, int> GetTypeLeaderboard();
        public Dictionary<string, int> GetLocationLeaderboard();
    }
}
