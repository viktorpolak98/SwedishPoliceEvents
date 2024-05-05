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
        const gridContainer = document.createElement("div");
        gridContainer.className = "grid-item-container";

        const upperGridItem = document.createElement("div");
        upperGridItem.className = "upper-grid-item";
        const upperItemText = document.createTextNode(event.city);
        upperGridItem.appendChild(upperItemText);

        const lowerGridItem = document.createElement("div");
        lowerGridItem.className = "lower-grid-item";
        const lowerItemText = document.createTextNode(event.title);
        lowerGridItem.appendChild(lowerItemText);

        gridContainer.appendChild(upperGridItem);
        gridContainer.appendChild(lowerGridItem);
        gridBox.appendChild(gridContainer);
    });
}
