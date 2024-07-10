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
    class PoliceEventsRepoInvalidInputTests : BaseTestFunctions
    {
        private PoliceEventsRepository _Repository;

        [SetUp]
        public void SetUp()
        {

            _Repository = new PoliceEventsRepository();
            _Repository.CreateValues(CreateTestDataDocument("TestEvents.json"));

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
