﻿using NUnit.Framework;
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
    class PoliceEventsRepoInvalidInputTests
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
        public void TestPoliceEventsFromInvalidLatLon()
        {
            string lat = "1";
            string lon = "1";

            List<PoliceEvent> list = _Repository.GetAllByLatLon(lat, lon);

            Assert.NotNull(list);
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void TestPoliceEventsFromInvalidLocation()
        {
            string locationName = "aaaaa";

            List<PoliceEvent> list = _Repository.GetAllByLocationName(locationName);

            Assert.NotNull(list);
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void TestGetPoliceEventFromInvalidId()
        {

            PoliceEvent policeEvent = _Repository.GetById("1");

            Assert.Null(policeEvent);
            Assert.Pass();
        }

        [Test]
        public void TestGetPoliceEventsFromNonExistingType()
        {
            List<PoliceEvent> eventsOfSpecificType = _Repository.GetAllByType("Förfalskningsbrott");

            Assert.NotNull(eventsOfSpecificType);
            Assert.AreEqual(0, eventsOfSpecificType.Count);

        }
    }
}
