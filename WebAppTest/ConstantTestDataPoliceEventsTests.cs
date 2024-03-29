﻿using NUnit.Framework;
using WebApp.Repositories;
using System.Text.Json;
using System;
using WebApp.Models.PoliceEvent;
using WebApp.Models.Shared;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using WebApp.HelperFunctions;

namespace WebAppTest
{
    class ConstantTestDataPoliceEventsTests
    {
        private readonly List<PoliceEvent> PoliceEvents = new();
        private JsonDocument doc;
        private Dictionary<string, EventType> EventTypeDict;
        private PoliceEventsRepository _Repository;

        [SetUp]
        public void SetUp()
        {
            string json;

            EventTypeDict = EnumValuesHelper.ToDictionaryDisplayNameAsKey<EventType>();

            string directory = Environment.CurrentDirectory;
            directory = Directory.GetParent(directory).Parent.Parent.FullName;

            using (StreamReader reader = new(directory + "\\TestData\\TestEvents.json"))
            {
                json = reader.ReadToEnd();
            }

            doc = JsonDocument.Parse(json);

            CreateEvents();

            _Repository = new PoliceEventsRepository(PoliceEvents);

        }

        public void CreateEvents()
        {
            PoliceEvents.Clear();

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
                    Type = EventTypeDict[Element.GetProperty("type").ToString()],
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
            List<PoliceEvent> eventsOfSpecificType = _Repository.GetAllByType(EventType.Rattfylleri);

            Assert.NotNull(eventsOfSpecificType);
            Assert.AreEqual(22, eventsOfSpecificType.Count);


            foreach (PoliceEvent e in eventsOfSpecificType)
            {
                Assert.AreEqual(e.Type, EventType.Rattfylleri);
            }

            Assert.Pass();

        }

        [Test]
        public void TestGetPoliceEventsFromTypeDisplayName()
        {
            string displayName = "Fylleri/LOB";
            List<PoliceEvent> list = _Repository.GetAllByDisplayName(displayName);

            Assert.NotNull(list);
            Assert.AreEqual(6, list.Count);

            foreach (PoliceEvent e in list)
            {
                Assert.AreEqual(e.Type, EventType.Fylleri_LOB);
            }

            Assert.Pass();

        }
    }
}
