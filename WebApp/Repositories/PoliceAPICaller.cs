using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Services;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace WebApp.Repositories
{
    public class PoliceAPICaller : IReadData<JsonDocument>
    {
        private readonly HttpClient Client;
        private readonly SemaphoreSlim RequestSemaphore;
        private readonly MemoryCache MemCache;

        public PoliceAPICaller()
        {
            Client = new HttpClient();
            RequestSemaphore = new SemaphoreSlim(1,1);
            MemCache = new MemoryCache(new MemoryCacheOptions());
        }

        public async Task<JsonDocument> ReadData(string path)
        {

            if (MemCache.TryGetValue("data", out JsonDocument doc))
            {
                return doc;
            }

            RequestSemaphore.Wait();
            int tries = 0;

            while (tries < 3)
            {
                try
                {
                    HttpResponseMessage response = await Client.GetAsync(path);

                    if (response.IsSuccessStatusCode)
                    {
                        doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                        break;
                    }
                }
                catch (HttpRequestException Ex)
                {
                    Console.WriteLine(Ex.Message);
                }
                finally { RequestSemaphore.Release(); }

            }

            using (var entry = MemCache.CreateEntry("data"))
            {
                entry.Value = doc;
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(10);
            }

            return doc;
        }



    }
}
