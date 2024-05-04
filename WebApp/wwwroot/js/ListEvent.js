class ListEvent {
    constructor(title, city) {
        this.title = title;
        this.city = city;
    }
}

export const events = [];

export function addEventToList(title, city) {
    events.push(new ListEvent(tite, city));
}

export function addItemsToGrid() {
    const eventListElement = document.getElementById("eventList");

    events.forEach(event => {
        const listItem = document.createElement("li");
        listItem.textContent = `${event.title} - ${event.city}`;
        eventListElement.appendChild(listItem);
    });
}
