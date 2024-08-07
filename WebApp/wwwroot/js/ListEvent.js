import { addTypeToLeaderboardMap, addItemsToLeaderboard, clearLeaderboard } from "../js/Leaderboard.js";

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

export async function getEventsByLocation() {

    const city = document.getElementById("text-input").value;


    const url = `${location.origin}/PoliceEvents/GetPoliceEventsByLocation/${city}`;

    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        mapOfEvents.clear();
        clearLeaderboard();
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
        addItemsToLeaderboard();

    } catch (error) {
        console.error('Error fetching police events:', error);
    }
}

function addEventToMap(ListEvent) {
    mapOfEvents.set(ListEvent.id, ListEvent)
}


function addItemsToGrid() {
    const gridBox = document.getElementById("gridbox");
    gridBox.innerHTML = ``;

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

        gridBox.appendChild(container);
        container.addEventListener("click", function () {
            clickItem(id);
        }, false);
    });
}

function clickItem(id) {

    const detailsChild = document.getElementById("detail-view-child");
    const detailsParent = document.getElementById("detail-view");

    const ListEvent = mapOfEvents.get(id);

    detailsChild.innerHTML = `
    <p> 
        <b>ID:</b> ${ListEvent.id} 
        <br /> 
        <b>Name of event:</b> ${ListEvent.eventname} 
        <br /> 
        <b>Summary:</b> ${ListEvent.summary}
        <br /> 
        <b>Category:</b> ${ListEvent.type}
        <br /> 
        <b>Location:</b> ${ListEvent.locationName}: ${ListEvent.locationGps}
        <br />
        <b>Link:</b> <a href="${ListEvent.url}" target="_blank">${ListEvent.url}</a>"
    </p>
    `
    detailsParent.classList.toggle('show');
}

export function closeDetails() {
    const details = document.getElementById("detail-view");
    details.classList.toggle('show');
}
