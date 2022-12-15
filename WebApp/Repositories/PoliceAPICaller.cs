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

        public PoliceAPICaller()
        {
            Client = new HttpClient();
        }

        public async Task<JsonDocument> ReadData(string path)
        {

            JsonDocument doc = null;

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

            }

            return doc;
        }



    }
}
