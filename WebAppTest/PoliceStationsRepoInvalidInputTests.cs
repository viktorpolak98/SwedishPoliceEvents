using NUnit.Framework;
using WebApp.Repositories;
using System.Text.Json;
using System;
using WebApp.Models.PoliceStation;
using System.Collections.Generic;
using System.IO;
using WebApp.Models.Shared;

namespace WebAppTest
{
    public class PoliceStationsRepoInvalidInputTests
    {
        private readonly List<PoliceStation> PoliceStations = [];
        private JsonDocument doc;
        private PoliceStationsRepository _Repository;

        [SetUp]
        public void SetUp()
        {
            CreateStations();

            _Repository = new PoliceStationsRepository(PoliceStations);

        }

        public void CreateStations()
        {
            PoliceStations.Clear();

            string json;

            string directory = Environment.CurrentDirectory;
            directory = Directory.GetParent(directory).Parent.Parent.FullName;


            using (StreamReader reader = new(directory + "\\TestData\\TestPoliceStations.json"))
            {
                json = reader.ReadToEnd();
            }

            doc = JsonDocument.Parse(json);

            foreach (JsonElement Element in doc.RootElement.EnumerateArray())
            {
                List<string> serviceTypes = new();
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
        public void TestPoliceStationsFromInvalidLatLon()
        {
            string lat = "1";
            string lon = "1";

            List<PoliceStation> list = _Repository.GetAllByLatLon(lat, lon);

            Assert.NotNull(list);
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void TestPoliceStationsFromInvalidLocation()
        {
            string locationName = "aaaaa";

            List<PoliceStation> list = _Repository.GetAllByLocationName(locationName);

            Assert.NotNull(list);
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void TestGetPoliceStationFromInvalidId()
        {

            PoliceStation policeStation = _Repository.GetById("1");

            Assert.Null(policeStation);
            Assert.Pass();
        }
    }
}
