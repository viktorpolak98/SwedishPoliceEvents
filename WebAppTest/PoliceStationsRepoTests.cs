﻿using NUnit.Framework;
using WebApp.Repositories;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using WebApp.Models.PoliceStation;
using System.Collections.Generic;
using System.IO;

namespace WebAppTest
{
    class PoliceStationsRepoTests : BaseTestFunctions
    {
        private PoliceStationsRepository _Repository;
        private JsonDocument doc;

        [SetUp]
        public void Setup()
        {
            _Repository = new PoliceStationsRepository();

            doc = CreateTestDataDocument("TestPoliceStations.json");

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

            Assert.AreEqual(261, _Repository.AmountOfCachedItems());
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
        public void CreatePoliceStationsTest()
        {
            _Repository.CreateValues(doc);

            Assert.True(_Repository.CacheIsFull());
            Assert.Pass();
        }
    }
}
