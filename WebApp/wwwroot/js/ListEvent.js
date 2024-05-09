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
        const container = document.createElement("div");
        container.className = "grid-item-container";

        const upperComponent = document.createElement("div");
        upperComponent.className = "upper-grid-item";
        upperComponent.textContent = event.city;

        const lowerComponent = document.createElement("div");
        lowerComponent.className = "lower-grid-item";
        lowerComponent.textContent = event.title;

        container.appendChild(upperComponent);
        container.appendChild(lowerComponent);

        gridBox.appendChild(container);
    });
}
