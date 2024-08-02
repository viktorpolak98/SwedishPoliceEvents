using NUnit.Framework;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Models.PoliceStation;
using WebApp.Repositories;

namespace WebAppTest;

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
    public void CreatedPoliceStationsAreCachedTest()
    {
        _Repository.CreateValues(doc);

        Assert.True(_Repository.CacheIsFull());
        Assert.Pass();
    }

    [Test]
    public void CreatePoliceStation()
    {
        _Repository.CreateValues(doc);
        PoliceStation station = _Repository.GetAll()[0];

        Assert.AreEqual("1233", station.Id);
        Assert.AreEqual("Alingsås", station.Name);
        Assert.AreEqual("https://polisen.se/om-polisen/kontakt/polisstationer/vastra-gotaland/alingsas/", station.Url);
        Assert.AreEqual("N Strömgatan 8, Alingsås", station.Location.Name);
        Assert.AreEqual("57.930105,12.529608", station.Location.GpsLocation.ToString());
        Assert.AreEqual("Anmälan", station.Services[0]);
        Assert.AreEqual("Vapen", station.Services[5]);
    }
}
