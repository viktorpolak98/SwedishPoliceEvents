// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
import { events, addEventToList } from "./ListEvent";


function addItemsToList() {
    const eventListElement = document.getElementById("eventList");

    events.forEach(event => {
        const listItem = document.createElement("li");
        listItem.textContent = `${event.title} - ${event.city}`;
        eventListElement.appendChild(listItem);
    });
}


