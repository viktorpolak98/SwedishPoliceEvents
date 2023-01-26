using NUnit.Framework;
using WebApp.Repositories;
using System.Text.Json;
using System;
using System.Threading.Tasks;
using WebApp.Models.PoliceStation;
using WebApp.Models.Shared;


namespace WebAppTest
{
    public class PoliceStationsRepoTests
    {
        PoliceStationsRepository _Repository;
        private readonly string UrlPath = "https://polisen.se/api/policestations";

        [SetUp]
        public void Setup()
        {
            _Repository = new PoliceStationsRepository();

        }


        [Test]
        public async Task ReadDataTest()
        {
            JsonDocument doc = await _Repository.ReadData(UrlPath);
            Assert.NotNull(doc);

            foreach (var Element in doc.RootElement.EnumerateArray())
            {
                Console.WriteLine($"{Element}");
            }

            doc.Dispose();

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
        public async Task CreatePoliceStationsTest()
        {
            await _Repository.CreateValues(UrlPath);

            Console.WriteLine(_Repository.GetCount());

            Assert.AreEqual(275, _Repository.GetCount());
            Assert.Pass();

        }

    }
}
