class ListEvent {
    constructor(id, datetime, name, summary, url, type, locationName, locationGps) {
        this.id = id;
        this.datetime = datetime;
        this.name = name;
        this.summary = summary;
        this.url = url;
        this.type = type;
        this.locationName = locationName;
        this.locationGps = locationGps;
    }
}

export const events = [];

export function addEventToList(id, datetime, name, summary, url, type, locationName, locationGps) {
    events.push(new ListEvent(id, datetime, name, summary, url, type, locationName, locationGps));
}

export function addItemsToGrid() {
    events.length = 0;
    const gridBox = document.getElementById("gridbox");
    gridBox.innerHTML = "";


    events.forEach(event => {
        const container = document.createElement("div");
        container.className = "grid-item-container";
        container.onclick = clickItem();

        const upperComponent = document.createElement("div");
        upperComponent.className = "upper-grid-item";
        upperComponent.textContent = event.locationName;

        const lowerComponent = document.createElement("div");
        lowerComponent.className = "lower-grid-item";
        lowerComponent.textContent = event.name;

        container.appendChild(upperComponent);
        container.appendChild(lowerComponent);

        gridBox.appendChild(container);
    });
}

export function clickItem() {
    console.log("click");
}
