using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Services;
using System;

namespace WebApp.Repositories
{
    /// <summary>
    /// Class used to call swedish police api for open data found at https://polisen.se/om-polisen/om-webbplatsen/oppna-data/
    /// </summary>
    public class PoliceAPICaller : IReadData<JsonDocument>
    {
        private readonly HttpClient _client;

        public PoliceAPICaller(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Async api call to read data.
        /// Tries to call an api a maximum of 3 times with 500ms between each call. Does not continue calling api after a successful call.
        /// </summary>
        /// <param name="path">Path to api to call</param>
        /// <returns></returns>
        public async Task<JsonDocument> ReadData(string path)
        {

            JsonDocument doc = null;

            int tries = 0;

            while (tries < 3)
            {
                try
                {
                    HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress+path);

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
                Thread.Sleep(500);

            }

            return doc;
        }



    }
}
