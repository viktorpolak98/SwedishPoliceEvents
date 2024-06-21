using NUnit.Framework;
using WebApp.Repositories;
using System.Text.Json;
using System;
using WebApp.Models.PoliceEvent;
using WebApp.Models.Shared;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

namespace WebAppTest
{
    class ConstantTestDataPoliceEventsTests
    {
        private readonly List<PoliceEvent> PoliceEvents = [];
        private PoliceEventsRepository _Repository;

        [SetUp]
        public void SetUp()
        {
            CreateEvents();
            _Repository = new PoliceEventsRepository(PoliceEvents);
        }

        public void CreateEvents()
        {
            PoliceEvents.Clear();

            string json;

            string directory = Environment.CurrentDirectory;
            directory = Directory.GetParent(directory).Parent.Parent.FullName;

            using (StreamReader reader = new(directory + "\\TestData\\TestEvents.json"))
            {
                json = reader.ReadToEnd();
            }

            JsonDocument doc = JsonDocument.Parse(json);

            foreach (JsonElement Element in doc.RootElement.EnumerateArray())
            {

                string[] gps = Element.GetProperty("location").GetProperty("gps").ToString().Split(",");

                PoliceEvents.Add(new PoliceEvent
                {
                    Id = Element.GetProperty("id").ToString(),
                    Date = DateTime.ParseExact(Element.GetProperty("datetime").ToString(), "yyyy-MM-dd H:mm:ss zzz", CultureInfo.InvariantCulture,
                         DateTimeStyles.None),
                    Name = Element.GetProperty("name").ToString(),
                    Summary = Element.GetProperty("summary").ToString(),
                    Url = Element.GetProperty("url").ToString(),
                    Type = Element.GetProperty("type").ToString(),
                    Location = new Location
                    {
                        Name = Element.GetProperty("location").GetProperty("name").ToString(),
                        GpsLocation = new GPSLocation
                        {
                            Latitude = gps[0],
                            Longitude = gps[1]
                        }
                    }
                });

            }


            doc.Dispose();

        }

        [Test]
        public void TestPoliceEventsFromLatLon()
        {
            string lat = "63.825847";
            string lon = "20.263035";

            List<PoliceEvent> list = _Repository.GetAllByLatLon(lat, lon);

            Assert.NotNull(list);
            Assert.AreEqual(12, list.Count);

            foreach (PoliceEvent pEvent in list)
            {
                Assert.AreEqual(pEvent.Location.GpsLocation.Latitude, lat);
                Assert.AreEqual(pEvent.Location.GpsLocation.Longitude, lon);
            }

            Assert.Pass();
        }

        [Test]
        public void TestPoliceEventsFromLocation()
        {
            string locationName = "Malmö";

            List<PoliceEvent> list = _Repository.GetAllByLocationName(locationName);

            Assert.NotNull(list);
            Assert.AreEqual(10, list.Count);


            foreach (PoliceEvent pEvent in list)
            {
                Assert.AreEqual(pEvent.Location.Name, locationName);
            }

            Assert.Pass();
        }

        [Test]
        public void TestGetPoliceEventFromId()
        {

            PoliceEvent policeEvent = _Repository.GetById("385732");

            Assert.NotNull(policeEvent);
            Assert.AreEqual("385732", policeEvent.Id);
        }

        [Test]
        public void TestGetPoliceEventsFromType()
        {
            string type = "Rattfylleri";

            List<PoliceEvent> eventsOfSpecificType = _Repository.GetAllByType(type);

            Assert.NotNull(eventsOfSpecificType);
            Assert.AreEqual(22, eventsOfSpecificType.Count);


            foreach (PoliceEvent e in eventsOfSpecificType)
            {
                Assert.AreEqual(e.Type, type);
            }

            Assert.Pass();

        }
    }
}
