﻿using NUnit.Framework;
using WebApp.Repositories;
using System.Text.Json;
using System;
using WebApp.Models.PoliceStation;
using WebApp.Models.Shared;
using System.IO;
using System.Collections.Generic;

namespace WebAppTest
{
    public class ConstantTestDataPoliceStationsTests
    {
        private readonly List<PoliceStation> PoliceStations = [];
        private PoliceStationsRepository _Repository;

        [SetUp]
        public void SetUp()
        {
            CreateEvents();

            _Repository = new PoliceStationsRepository(PoliceStations);

        }


        public void CreateEvents()
        {
            PoliceStations.Clear();

            string json;

            string directory = Environment.CurrentDirectory;
            directory = Directory.GetParent(directory).Parent.Parent.FullName;

            using (StreamReader reader = new(directory + "\\TestData\\TestPoliceStations.json"))
            {
                json = reader.ReadToEnd();
            }

            JsonDocument doc = JsonDocument.Parse(json);

            foreach (JsonElement Element in doc.RootElement.EnumerateArray())
            {

                List<string> serviceTypes = [];
                foreach (JsonElement service in Element.GetProperty("services").EnumerateArray())
                {
                    serviceTypes.Add(service.GetProperty("name").ToString());
                }

                string[] gps = Element.GetProperty("location").GetProperty("gps").ToString().Split(",");


                PoliceStation station = new()
                {
                    Id = Element.GetProperty("id").ToString(),
                    Name = Element.GetProperty("name").ToString(),
                    Url = Element.GetProperty("Url").ToString(),
                    Location = new Location
                    {
                        Name = Element.GetProperty("location").GetProperty("name").ToString(),
                        GpsLocation = new GPSLocation
                        {
                            Latitude = gps[0],
                            Longitude = gps[1]
                        }
                    },
                    Services = serviceTypes
                };

                PoliceStations.Add(station);
            }

            doc.Dispose();
        }

        [Test]
        public void TestPoliceStationsFromLatLon()
        {
            string lat = "64.594354";
            string lon = "18.679252";

            List<PoliceStation> list = _Repository.GetAllByLatLon(lat, lon);

            Assert.NotNull(list);
            Assert.AreEqual(1, list.Count);

            foreach (PoliceStation pStation in list)
            {
                Assert.AreEqual(pStation.Location.GpsLocation.Latitude, lat);
                Assert.AreEqual(pStation.Location.GpsLocation.Longitude, lon);
            }

            Assert.Pass();
        }

        [Test]
        public void TestPoliceStationsFromLocation()
        {
            string locationName = "Folkungavägen 2, Nyköping";

            List<PoliceStation> list = _Repository.GetAllByLocationName(locationName);

            Assert.NotNull(list);
            Assert.AreEqual(1, list.Count);


            foreach (PoliceStation pStation in list)
            {
                Assert.AreEqual(pStation.Location.Name, locationName);
            }

            Assert.Pass();
        }

        [Test]
        public void TestGetPoliceStationFromId()
        {

            PoliceStation policeEvent = _Repository.GetById("1124");

            Assert.NotNull(policeEvent);
            Assert.AreEqual("1124", policeEvent.Id);
        }

        [Test]
        public void TestGetPoliceStationsFromType()
        {
            string type = "Delgivning";
            List<PoliceStation> stationsOfSpecificType = _Repository.GetAllByType(type);

            Assert.NotNull(stationsOfSpecificType);
            Assert.AreEqual(28, stationsOfSpecificType.Count);


            foreach (PoliceStation e in stationsOfSpecificType)
            {
                Assert.True(e.Services.Contains(type));
            }

            Assert.Pass();

        }
    }
}
