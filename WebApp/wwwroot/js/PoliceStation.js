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

function addStationToMap(PoliceStation) {
    mapOfStations.set(PoliceStation.id, PoliceStation);
}

export async function getAllStations() {
    const url = `${location.origin}/PoliceStations/GetAllPoliceStations`;

    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        mapOfStations.clear();
        const data = await response.json();
        data.forEach(item => {
            const station = new PoliceStation(
                item.id,
                item.name,
                item.url,
                item.location.name,
                `${item.location.gpsLocation.latitude},${item.location.gpsLocation.longitude}`,
                item.services
            );
            addStationToMap(station);
        });
    } catch (error) {
        console.error('Error fetching police stations', error);
    }
}