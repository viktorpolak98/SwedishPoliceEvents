class ListEvent {
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

export function addEventToMap(ListEvent) {
    mapOfEvents.set(ListEvent.id, ListEvent)
}

export function addItemsToGrid() {
    events.length = 0;
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
    }

    const details = document.getElementById("detail-view");
    ListEvent = mapOfEvents.get(id);

    details.innerHTML = `${ListEvent.id}
    ${ListEvent.eventname} 
    ${ListEvent.summary}
    ${ListEvent.type}
    ${ListEvent.locationName} ${ListEvent.locationGps} 
    `
}
