using NUnit.Framework;
using System.Collections.Generic;
using WebApp.Models.PoliceStation;
using WebApp.Repositories;

namespace WebAppTest;

class PoliceStationsRepoInvalidInputTests : BaseTestFunctions
{
    private PoliceStationsRepository _Repository;

    [SetUp]
    public void SetUp()
    {
        _Repository = new PoliceStationsRepository();
        _Repository.CreateValues(CreateTestDataDocument("TestPoliceStations.json"));
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
