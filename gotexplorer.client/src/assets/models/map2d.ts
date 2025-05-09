import * as L from 'leaflet';

const customIcon = new L.Icon({
  iconUrl: '/assets/marker-icon.png',
  iconRetinaUrl: '/assets/marker-icon-2x.png', 
  shadowUrl: '/assets/marker-shadow.png',
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41]
});
export class Map2d {
    private map: L.Map;
    private imageBounds: [[number, number], [number, number]];
    private imageUrl: string;
    private mapClickCallback!: (lat: number, lng: number) => void;
    private markers: L.Marker[] = []; 

    constructor(imageUrl: string, imageBounds: [[number, number], [number, number]], containerId: string) {
        this.imageUrl = imageUrl;
        this.imageBounds = imageBounds;
        this.map = L.map(containerId, {
            crs: L.CRS.Simple,
            minZoom: -2,
            maxZoom: 2,
            center: this.calculateCenter(imageBounds),
            zoom: 1,
            maxBounds: imageBounds,
            bounceAtZoomLimits: false,
            inertia: true
        });

        this.addImageOverlay();
        this.setupEventListeners();
    }

    private calculateCenter(bounds: [[number, number], [number, number]]): L.LatLng {
        const latCenter = (bounds[0][0] + bounds[1][0]) / 2;
        const lngCenter = (bounds[0][1] + bounds[1][1]) / 2;
        return new L.LatLng(latCenter, lngCenter);
    }

    private addImageOverlay(): void {
        L.imageOverlay(this.imageUrl, this.imageBounds).addTo(this.map);
        this.map.fitBounds(this.imageBounds);
    }

    private setupEventListeners(): void {
        this.map.on('click', this.handleMapClick.bind(this));
        
        this.map.on('zoomend', () => {
            if (this.map.getZoom() === this.map.getMinZoom()) {
                this.map.dragging.disable();
            } else {
                this.map.dragging.enable();
            }
        });
        
        this.map.on('moveend', () => {
            if (!this.isMapWithinOuterBounds()) {
                this.map.panInsideBounds(this.imageBounds, { animate: true });
            }
        });

        this.map.on('drag', () => {
            this.map.panInsideBounds(this.imageBounds, { animate: false });
        });
        
        if (this.map.getZoom() === this.map.getMinZoom()) {
            this.map.dragging.disable();
        }
    }

    private isMapWithinOuterBounds(): boolean {
        const mapBounds = this.map.getBounds();
        const sw = mapBounds.getSouthWest();
        const ne = mapBounds.getNorthEast();
        
        return (sw.lat >= this.imageBounds[0][0] && sw.lng >= this.imageBounds[0][1] &&
                ne.lat <= this.imageBounds[1][0] && ne.lng <= this.imageBounds[1][1]);
    }

    public addMarker(lat: number, lng: number): L.Marker {
        const marker = L.marker([lat, lng], { icon: customIcon }).addTo(this.map);
          this.markers.push(marker);
        return marker; 
    }


    public handleMapClick(e: L.LeafletMouseEvent): void {
        const coords = e.latlng;
        if (
            coords.lng >= this.imageBounds[0][1] && coords.lng <= this.imageBounds[1][1] &&
            coords.lat >= this.imageBounds[0][0] && coords.lat <= this.imageBounds[1][0]
        ) {
            if (this.mapClickCallback) {
                this.mapClickCallback(coords.lat, coords.lng);
            }
        }
    }

    public onMapClick(callback: (lat: number, lng: number) => void): void {
        this.mapClickCallback = callback;
    }

    public showPopup(lat: number, lng: number, message: string): void {
        const marker = L.marker([lat, lng])
            .addTo(this.map)
            .bindPopup(message)
            .openPopup();

        this.markers.push(marker); 
    }
    

    public clearMarkers(): void {
        this.markers.forEach((marker) => {
            this.map.removeLayer(marker);
        });
        this.markers = [];
    }

    public removeMarker(marker: L.Marker): void {
        this.map.removeLayer(marker); 
    }

    public handleContainerResize(): void {
        this.map.invalidateSize();
        this.map.setView(this.calculateCenter(this.imageBounds), this.map.getZoom());
    }
}