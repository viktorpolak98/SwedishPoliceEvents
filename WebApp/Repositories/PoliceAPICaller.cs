using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using WebApp.Models;
using System.Text.Json;
using WebApp.HelperFunctions;
using System.Globalization;

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
