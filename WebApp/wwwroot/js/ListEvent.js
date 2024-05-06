class ListEvent {
    constructor(title, city) {
        this.title = title;
        this.city = city;
    }
}

export const events = [];

export function addEventToList(title, city) {
    events.push(new ListEvent(title, city));
}

export function addItemsToGrid() {
    events.length = 0;
    const gridBox = document.getElementById("gridbox");
    gridBox.innerHTML = "";


    events.forEach(event => {
        const eventElement = document.createElement("event-component");
        eventElement.updateUpperText(event.city);
        eventElement.updateLowerText(event.title);

        gridBox.appendChild(eventElement);
    });
}
