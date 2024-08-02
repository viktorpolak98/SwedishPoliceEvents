using NUnit.Framework;
using System.Collections.Generic;
using WebApp.Models.PoliceStation;
using WebApp.Repositories;

namespace WebAppTest;

class ConstantTestDataPoliceStationsTests : BaseTestFunctions
{
    private PoliceStationsRepository _Repository;

    [SetUp]
    public void SetUp()
    {

        _Repository = new PoliceStationsRepository();
        _Repository.CreateValues(CreateTestDataDocument("TestPoliceStations.json"));

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
