using NUnit.Framework;
using WebApp.Repositories;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using WebApp.Models.PoliceEvent;
using WebApp.Models.Shared;
using System.Collections.Generic;
using System.IO;

namespace WebAppTest
{
    class PoliceEventsRepoTests : BaseTestFunctions
    {
        private PoliceEventsRepository _Repository;
        private JsonDocument doc;

        [SetUp]
        public void Setup()
        {
            _Repository = new PoliceEventsRepository();
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
            doc = CreateTestDataDocument("TestEvents.json");

            _ = Task.Run(() => _Repository.CreateValues(doc));
            await Task.Run(() => _Repository.CreateValues(doc));

            Assert.True(_Repository.CacheIsFull());
        }
        
        
        [Test]
        public void ValidateCachedItemsTest()
        {
            doc = CreateTestDataDocument("TestEvents.json");

            _Repository.CreateValues(doc);
            List<PoliceEvent> validateEventsList = _Repository.ValidateEntries();
            foreach(var val in validateEventsList)
            {
                Assert.IsNotNull(val);
            }
            Assert.Pass();
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
            doc = CreateTestDataDocument("SingleEvent.json");

            Assert.NotNull(doc);

            DateTime date = _Repository.DateConverter("2022-11-30 13:30:00 +01:00");

            _Repository.CreateValues(doc);

            PoliceEvent policeEvent = _Repository.GetAll()[0];

            Assert.NotNull(policeEvent);

            Assert.AreEqual("387116", policeEvent.Id);
            Assert.AreEqual(date, policeEvent.Date);
            Assert.AreEqual("30 november 13:29, Rattfylleri, Eskilstuna", policeEvent.Name);
            Assert.AreEqual("Misstänkt drograttfylla vid kontroll i Eskilstuna.", policeEvent.Summary);
            Assert.AreEqual("/aktuellt/handelser/2022/november/30/30-november-1329-rattfylleri-eskilstuna/", policeEvent.Url);
            Assert.AreEqual("Rattfylleri", policeEvent.Type);
            Assert.AreEqual("Eskilstuna", policeEvent.Location.Name);
            Assert.AreEqual("59.371249,16.509805", $"{policeEvent.Location.GpsLocation.Latitude},{policeEvent.Location.GpsLocation.Longitude}");

            Assert.Pass();
        }

        [Test]
        public void CreatedPoliceEventsAreCachedTest()
        {
            doc = CreateTestDataDocument("TestEvents.json");
            _Repository.CreateValues(doc);

            Assert.AreEqual(500, _Repository.GetCount());
            Assert.Pass();
        }
    }
}