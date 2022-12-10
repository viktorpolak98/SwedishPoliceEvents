using NUnit.Framework;
using WebApp.Repositories;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebAppTest
{
    public class PoliceEventsRepoTests
    {
        PoliceEventsRepository _Repository;
        private readonly string UrlPath = "https://polisen.se/api/events";

        [SetUp]
        public void Setup()
        {
            _Repository = new PoliceEventsRepository();

        }


        [Test]
        public async Task ReadDataTest()
        {
            JsonDocument doc = await _Repository.ReadData("https://polisen.se/api/events"); 
            Assert.NotNull(doc);

            foreach(var Element in doc.RootElement.EnumerateArray())
            {
                Console.WriteLine($"{Element}");
            }

            doc.Dispose();

            Assert.Pass();
        }

        [Test]
        public void TestGetEventType()
        {
            string displayName = "Kontroll person/fordon";
            EventType type = _Repository.GetEventType(displayName);

            Assert.AreEqual(type, EventType.Kontroll_person_fordon);

        }

        [Test]
        public void DateParseTest()
        {

            string dateString = "2022-11-30 9:47:29 +01:00";


            DateTime dateValue = _Repository.DateConverter(dateString);

            Assert.AreEqual("2022-11-30 09:47:29", dateValue.ToString());
        }

        [Test]
        public async Task CreatePoliceEvent()
        {
            JsonDocument doc = await _Repository.ReadData(UrlPath);
            Assert.NotNull(doc);

            JsonElement Element = doc.RootElement[0];

            Console.WriteLine(Element.ToString());

            
            string[] gps = Element.GetProperty("location").GetProperty("gps").ToString().Split(",");


            PoliceEvent policeEvent = new PoliceEvent()
            {
                Id = Element.GetProperty("id").ToString(),
                Date = _Repository.DateConverter(Element.GetProperty("datetime").ToString()),
                Name = Element.GetProperty("name").ToString(),
                Summary = Element.GetProperty("summary").ToString(),
                Url = Element.GetProperty("url").ToString(),
                Type = _Repository.GetEventType(Element.GetProperty("type").ToString()),
                Location = new Location
                {
                    Name = Element.GetProperty("location").GetProperty("name").ToString(),
                    GpsLocation = new GPSLocation
                    {
                        Latitude = gps[0],
                        Longitude = gps[1]
                    }
                }
            };

            Console.WriteLine(policeEvent.ToString());

            Assert.NotNull(policeEvent); 
            Assert.Pass();
        }

        [Test]
        public async Task CreatePoliceEventsTest()
        {
            await _Repository.CreateEvents(UrlPath);

            Console.WriteLine(_Repository.GetNumberOfPoliceEvents());

            Assert.AreEqual(500, _Repository.GetNumberOfPoliceEvents());
            Assert.Pass();

        }
    }
}