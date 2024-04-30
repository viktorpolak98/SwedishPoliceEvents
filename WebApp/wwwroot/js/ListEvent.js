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

