import { Map2d } from './map2d';
import { toast } from 'sonner';
export class GameLogic {
    private map: Map2d;
    private score: number;
    private lastClick: { lat: number; lng: number } | null;
    private lastMarker: L.Marker | null;

    constructor(map: Map2d) {
        this.map = map;
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
        // if (this.lastMarker) {
        //     this.lastMarker.bindPopup(`Coordinates: (${lat.toFixed(2)}, ${lng.toFixed(2)})`).openPopup();
        // }

        this.lastClick = { lat, lng };
    }

    public hasMarker(): boolean {
        return this.lastMarker !== null;
    }


    public submitAnswer(): string {
        if (!this.lastClick) {
            toast.error('No selection made! Please click on the map first.')
            return 'No selection made! Please click on the map first.';
        }
        return "";
    }
    public getClick() {
        return this.lastClick;
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