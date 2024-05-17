class PoliceStation {
    constructor(id, stationName, url, locationName, locationGps, services = []) {
        this.id = id;
        this.stationName = stationName;
        this.url = url;
        this.locationName = locationName;
        this.locationGps = locationGps;
        this.services = services;
    }
}

const mapOfStations = new Map();

export function addStationToMap(PoliceStation) {
    mapOfStations.set(PoliceStation.id, PoliceStation);
}