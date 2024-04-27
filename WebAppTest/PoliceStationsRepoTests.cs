using NUnit.Framework;
using WebApp.Repositories;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using WebApp.Models.PoliceStation;
using System.Collections.Generic;
using System.IO;

namespace WebAppTest
{
    public class PoliceStationsRepoTests
    {
        private PoliceStationsRepository _Repository;
        private JsonDocument doc;

        [SetUp]
        public void Setup()
        {
            _Repository = new PoliceStationsRepository();
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

            Assert.AreEqual(276, _Repository.AmountOfCachedItems());
        }


        [Test]
        public void ValidateCachedItemsTest()
        {
            _Repository.CreateValues(doc);
            List<PoliceStation> validateEventsList = _Repository.ValidateEntries();
            foreach (var val in validateEventsList)
            {
                Assert.IsNotNull(val);
            }
            Assert.Pass();
        }

        [Test]
        public void TestGetServiceType()
        {
            string displayName = "Provisoriskt pass";
            ServiceType type = _Repository.GetType(displayName);

            Assert.AreEqual(type, ServiceType.Provisoriskt_pass);

        }

        [Test]
        public void CreatePoliceStationsTest()
        {
            _Repository.CreateValues(doc);

            Assert.AreEqual(275, _Repository.GetCount());
            Assert.Pass();
        }
    }
}
