using System.Threading.Tasks;
using WebApp.Services;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApp.Repositories
{
    public class PoliceAPICaller : IReadData<JsonDocument>
    {
        private readonly HttpClient Client;

        public PoliceAPICaller()
        {
            Client = new HttpClient();
        }

        public async Task<JsonDocument> ReadData(string location)
        {
            JsonDocument doc = null;
            HttpResponseMessage response = await Client.GetAsync(location);
            if (response.IsSuccessStatusCode)
            {
                doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            }

            return doc;
        }


    }
}
