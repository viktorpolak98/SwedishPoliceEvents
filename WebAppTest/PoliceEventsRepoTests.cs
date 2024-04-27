using NUnit.Framework;
using WebApp.Repositories;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using WebApp.Models.PoliceEvent;
using WebApp.Models.Shared;
using System.Threading;
using System.Collections.Generic;
using System.IO;

namespace WebAppTest
{
    public class PoliceEventsRepoTests
    {
        private PoliceEventsRepository _Repository;
        private JsonDocument doc;

        [SetUp]
        public void Setup()
        {
            _Repository = new PoliceEventsRepository();
            
            string json;
            string directory = Environment.CurrentDirectory;
            directory = Directory.GetParent(directory).Parent.Parent.FullName;


            using (StreamReader reader = new(directory + "\\TestData\\TestEvents.json"))
            {
                json = reader.ReadToEnd();
            }

            doc = JsonDocument.Parse(json);

        }

        [TearDown]
        public void TearDown()
        {
            doc.Dispose();
        }

        //Should be ran by itself to ensure that MemCache is empty when executing test
        [Test]
        public async Task MultiThreadCreateValuesTest()
        {

            _ = Task.Run(() => _Repository.CreateValues(doc));
            await Task.Run(() => _Repository.CreateValues(doc));

            Assert.AreEqual(500, _Repository.AmountOfCachedItems());
        }
        
        
        [Test]
        public void ValidateCachedItemsTest()
        {

            _Repository.CreateValues(doc);
            List<PoliceEvent> validateEventsList = _Repository.ValidateEntries();
            foreach(var val in validateEventsList)
            {
                Assert.IsNotNull(val);
            }
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
        public void CreatePoliceEvent()
        {
            Assert.NotNull(doc);

            JsonElement Element = doc.RootElement[0];

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

            Assert.NotNull(policeEvent);
            Assert.Pass();
        }

        [Test]
        public void CreatePoliceEventsTest()
        {
            _Repository.CreateValues(doc);

            Assert.AreEqual(500, _Repository.GetCount());
            Assert.Pass();
        }
    }
}