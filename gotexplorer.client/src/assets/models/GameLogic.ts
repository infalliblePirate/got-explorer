import { Map2d } from './map2d';

export class GameLogic {
    private map: Map2d;
    private targetLocation: { lat: number; lng: number };
    private radius: number;
    private score: number;
    private lastClick: { lat: number; lng: number } | null;
    private lastMarker: L.Marker | null; 

    constructor(map: Map2d, targetLocation: { lat: number; lng: number }, radius: number) {
        this.map = map;
        this.targetLocation = targetLocation;
        this.radius = radius;
        this.score = 0;
        this.lastClick = null; 
        this.lastMarker = null; 

        this.map.onMapClick(this.handleMapClick.bind(this));
    }

    private handleMapClick(lat: number, lng: number): void {

        if (this.lastMarker) {
            this.map.removeMarker(this.lastMarker); 
            this.lastMarker = null; 
        }

        this.lastMarker = this.map.addMarker(lat, lng);

        // show the coordinates
        if (this.lastMarker) {
            this.lastMarker.bindPopup(`Coordinates: (${lat.toFixed(2)}, ${lng.toFixed(2)})`).openPopup();
        }

        this.lastClick = { lat, lng };
    }

    public hasMarker(): boolean {
        return this.lastMarker !== null; 
    }


    public submitAnswer(): string {
        if (!this.lastClick) {
            return 'No selection made! Please click on the map first.';
        }

        const distance = Math.sqrt(
            Math.pow(this.lastClick.lat - this.targetLocation.lat, 2) +
            Math.pow(this.lastClick.lng - this.targetLocation.lng, 2)
        );

        const isCorrect = distance < this.radius;

        if (isCorrect) {
            this.score += 10; 
            return `Correct! +10 points. Current score: ${this.score}`;
        } else {
            return `Incorrect! No points. Current score: ${this.score}`;
        }
    }

    public reset(): void {
        this.lastClick = null;
        this.score = 0;
        if (this.lastMarker) {
            this.map.removeMarker(this.lastMarker); 
            this.lastMarker = null;
        }
    }

    public getScore(): number {
        return this.score;
    }
}
