using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Services;

namespace WebApp.Repositories;

/// <summary>
/// Class used to call swedish police api for open data found at https://polisen.se/om-polisen/om-webbplatsen/oppna-data/
/// </summary>
public class PoliceAPICaller : IReadData<JsonDocument>
{
    private readonly HttpClient _client;

#pragma warning disable IDE0290
    public PoliceAPICaller(HttpClient client)
    {
        _client = client;
    }

    public async Task<JsonDocument> ReadData(string path)
    {

        JsonDocument doc = null;

        int tries = 0;

        while (tries < 3)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + path);

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
