﻿class ListEvent {
    constructor(id, datetime, eventname, summary, url, type, locationName, locationGps) {
        this.id = id;
        this.datetime = datetime;
        this.name = eventname;
        this.summary = summary;
        this.url = url;
        this.type = type;
        this.locationName = locationName;
        this.locationGps = locationGps;
    }
}

const mapOfEvents = new Map();

async function getEventsByLocation() {
    console.log("getEvents")

    const city = document.getElementById("text-input").value;
    console.log(city);


    const url = `${location.origin}/PoliceEvent/GetPoliceEventsByLocation/${city}`;

    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        console.log("data: \n" + data);
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

            addEventToMap(event.id, event); 
        });

        addItemsToGrid();

    } catch (error) {
        console.error('Error fetching police events:', error);
    }
}

function addEventToMap(ListEvent) {
    mapOfEvents.set(ListEvent.id, ListEvent)
}

function addItemsToGrid() {
    console.log("addItemsToGrid");
    const gridBox = document.getElementById("gridbox");
    gridBox.innerHTML = "";


    mapOfEvents.forEach(id, event => {
        const container = document.createElement("div");
        container.className = "grid-item-container";
        
        const upperComponent = document.createElement("div");
        upperComponent.className = "upper-grid-item";
        upperComponent.textContent = event.id;

        const lowerComponent = document.createElement("div");
        lowerComponent.className = "lower-grid-item";
        lowerComponent.textContent = event.eventname;

        container.appendChild(upperComponent);
        container.appendChild(lowerComponent);

        container.onclick = clickItem(upperComponent.textContent);
        gridBox.appendChild(container);
    });
}

function clickItem(id) {
    const flexbox = document.getElementById("flexcontainer");

    if (!document.contains(document.getElementById("detail-view"))) {
        let details = document.createElement("div");
        details.className = "detail-view";
        details.id = "detail-view";
        flexbox.appendChild(details);

        const closeButton = document.createElement('button');

        closeButton.className = 'close-button';
        closeButton.setAttribute('aria-label', 'Close alert');
        closeButton.setAttribute('type', 'button');
        closeButton.setAttribute('data-close', '');
        closeButton.id = "close-detail-view";

        const span = document.createElement('span');

        span.setAttribute('aria-hidden', 'true');
        span.innerHTML = '&times;';

        closeButton.appendChild(span);
    }

    const details = document.getElementById("detail-view");
    const closeButton = document.getElementById("close-detail-view");
    ListEvent = mapOfEvents.get(id);

    details.innerHTML = `${ListEvent.id} ${closeButton}
    ${ListEvent.eventname} 
    ${ListEvent.summary}
    ${ListEvent.type}
    ${ListEvent.locationName} ${ListEvent.locationGps} 
    `
}
