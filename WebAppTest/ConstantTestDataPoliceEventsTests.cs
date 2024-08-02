using NUnit.Framework;
using System.Collections.Generic;
using WebApp.Models.PoliceEvent;
using WebApp.Repositories;

namespace WebAppTest;

class ConstantTestDataPoliceEventsTests : BaseTestFunctions
{
    private PoliceEventsRepository _Repository;

    [SetUp]
    public void SetUp()
    {
        _Repository = new PoliceEventsRepository();

        _Repository.CreateValues(CreateTestDataDocument("TestEvents.json"));

    }

    [Test]
    public void TestPoliceEventsFromLatLon()
    {
        string lat = "63.825847";
        string lon = "20.263035";

        List<PoliceEvent> list = _Repository.GetAllByLatLon(lat, lon);

        Assert.NotNull(list);
        Assert.AreEqual(12, list.Count);

        foreach (PoliceEvent pEvent in list)
        {
            Assert.AreEqual(pEvent.Location.GpsLocation.Latitude, lat);
            Assert.AreEqual(pEvent.Location.GpsLocation.Longitude, lon);
        }

        Assert.Pass();
    }

    [Test]
    public void TestPoliceEventsFromLocation()
    {
        string locationName = "Malmö";

        List<PoliceEvent> list = _Repository.GetAllByLocationName(locationName);

        Assert.NotNull(list);
        Assert.AreEqual(10, list.Count);


        foreach (PoliceEvent pEvent in list)
        {
            Assert.AreEqual(pEvent.Location.Name, locationName);
        }

        Assert.Pass();
    }

    [Test]
    public void TestGetPoliceEventFromId()
    {

        PoliceEvent policeEvent = _Repository.GetById("385732");

        Assert.NotNull(policeEvent);
        Assert.AreEqual("385732", policeEvent.Id);
    }

    [Test]
    public void TestGetPoliceEventsFromType()
    {
        string type = "Rattfylleri";

        List<PoliceEvent> eventsOfSpecificType = _Repository.GetAllByType(type);

        Assert.NotNull(eventsOfSpecificType);
        Assert.AreEqual(22, eventsOfSpecificType.Count);


        foreach (PoliceEvent e in eventsOfSpecificType)
        {
            Assert.AreEqual(e.Type, type);
        }

        Assert.Pass();

    }
}
