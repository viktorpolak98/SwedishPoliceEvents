class EventComponent extends HTMLElement {
    constructor() {
        super();

        this.attachShadow({ mode: 'open' });

        const container = document.createElement('div');
        this.container.className = 'grid-item-container';

        this.upperComponent = document.createElement('div');
        this.upperComponent.className = 'upper-grid-item';
        this.lowerComponent = document.createElement('div');
        this.lowerComponent.className = 'lower-grid-item';

        this.container.appendChild(upperComponent);
        this.container.appendChild(lowerComponent);

        this.shadowRoot.appendChild(container);
    }

    updateUpperText(text) {
        this.upperComponent.textContent = text;
    }

    updateLowerText(text) {
        this.lowerComponent.textContent = text; 
    }
}

customElements.define('event-component', EventComponent);
