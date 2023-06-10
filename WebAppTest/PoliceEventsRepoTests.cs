using NUnit.Framework;
using WebApp.Repositories;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using WebApp.Models.PoliceEvent;
using WebApp.Models.Shared;
using System.Threading;
using System.Collections.Generic;

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

        //Should be ran by itself to ensure that MemCache is empty when executing test
        [Test]
        public async Task MultiThreadCreateValuesTest()
        {
            _ = Task.Run(() => _Repository.CreateValues(UrlPath));
            await Task.Run(() => _Repository.CreateValues(UrlPath));

            Assert.AreEqual(500, _Repository.AmountOfCachedItems());
        }
        
        
        [Test]
        public async Task ValidateCachedItemsTest()
        {
            await _Repository.CreateValues(UrlPath);
            List<PoliceEvent> validateEventsList = _Repository.ValidateEntries();
            Console.WriteLine(_Repository.AmountOfCachedItems());
            foreach(var val in validateEventsList)
            {
                Assert.IsNotNull(val);
                Console.WriteLine(val);
            }
            Assert.Pass();
        }
        


        [Test]
        public async Task ReadDataTest()
        {
            JsonDocument doc = await _Repository.ReadData("https://polisen.se/api/events");
            Assert.NotNull(doc);

            foreach (var Element in doc.RootElement.EnumerateArray())
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
            EventType type = _Repository.GetType(displayName);

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


            PoliceEvent policeEvent = new()
            {
                Id = Element.GetProperty("id").ToString(),
                Date = _Repository.DateConverter(Element.GetProperty("datetime").ToString()),
                Name = Element.GetProperty("name").ToString(),
                Summary = Element.GetProperty("summary").ToString(),
                Url = Element.GetProperty("url").ToString(),
                Type = _Repository.GetType(Element.GetProperty("type").ToString()),
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
            await _Repository.CreateValues(UrlPath);

            Console.WriteLine(_Repository.GetCount());

            Assert.AreEqual(500, _Repository.GetCount());
            Assert.Pass();

        }


    }
}