class ListEvent {
    constructor(id, datetime, eventname, summary, url, type, locationName, locationGps) {
        this.id = id;
        this.datetime = datetime;
        this.eventname = eventname;
        this.summary = summary;
        this.url = url;
        this.type = type;
        this.locationName = locationName;
        this.locationGps = locationGps;
    }
}

const mapOfEvents = new Map();
const mapForLeaderboard = new Map();

async function getEventsByLocation() {

    const city = document.getElementById("text-input").value;


    const url = `${location.origin}/PoliceEvents/GetPoliceEventsByLocation/${city}`;

    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        mapOfEvents.clear();
        const data = await response.json();
        data.forEach(item => {
            const event = new ListEvent(
                item.id, 
                item.date, 
                item.name, 
                item.summary, 
                item.url,
                item.type, 
                item.location.name, 
                `${item.location.gpsLocation.latitude},${item.location.gpsLocation.longitude}`
            );
            
            addEventToMap(event);
            addTypeToLeaderboardMap(item.type);
        });

        addItemsToGrid();

    } catch (error) {
        console.error('Error fetching police events:', error);
    }
}

function addEventToMap(ListEvent) {
    mapOfEvents.set(ListEvent.id, ListEvent)
}

function addTypeToLeaderboardMap(type) {
    if (mapForLeaderboard.has(type)) {
        mapForLeaderboard.set(type, mapForLeaderboard.get(type) + 1);
        return;
    } 

    mapForLeaderboard.set(type, 1);
}

function addItemsToGrid() {
    const gridBox = document.getElementById("gridbox");
    gridBox.innerHTML = "";

    mapOfEvents.forEach((event, id) => { //Value, Key for some dumb reason 
        const container = document.createElement("div");
        container.className = "grid-item-container";
        
        const upperComponent = document.createElement("div");
        upperComponent.className = "upper-grid-item";
        upperComponent.textContent = id;

        const lowerComponent = document.createElement("div");
        lowerComponent.className = "lower-grid-item";
        lowerComponent.textContent = event.eventname;

        container.appendChild(upperComponent);
        container.appendChild(lowerComponent);

       // container.onclick = clickItem(upperComponent.textContent);
        gridBox.appendChild(container);
    });
}

function clickItem(id) {

    const details = document.getElementById("detail-view");
    const closeButton = document.getElementById("close-detail-view");
    ListEvent = mapOfEvents.get(id);

    details.innerHTML = `${ListEvent.id} ${closeButton}
    ${ListEvent.eventname} 
    ${ListEvent.summary}
    ${ListEvent.type}
    ${ListEvent.locationName} ${ListEvent.locationGps} 
    `

    details.hidden = false; 
}

function detailsClose() {
    const details = document.getElementById("detail-view");
    details.hidden = true;
}
