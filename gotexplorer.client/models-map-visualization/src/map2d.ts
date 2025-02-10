import * as L from 'leaflet';

const IMAGE_BOUNDS: L.LatLngBoundsExpression = [[0, 0], [1080, 720]];

const map = L.map('map', {
    crs: L.CRS.Simple,
    minZoom: -2,
    maxZoom: 2,
    center: [540, 360],
    zoom: 1,
});

L.imageOverlay('assets/map.png', IMAGE_BOUNDS).addTo(map);

map.fitBounds(IMAGE_BOUNDS);

map.on('click', function (e) {
    const coords = e.latlng;
    if (coords.lng >= IMAGE_BOUNDS[0][1] && coords.lng <= IMAGE_BOUNDS[1][1]
        && coords.lat >= IMAGE_BOUNDS[0][0] && coords.lat <= IMAGE_BOUNDS[1][0]) {

        L.marker([coords.lat, coords.lng]).addTo(map)
            .bindPopup(`Coordinates: x=${coords.lng}, y=${coords.lat}`).openPopup();
    }
});



