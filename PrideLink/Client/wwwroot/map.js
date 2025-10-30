function initializeMap() {
    mapboxgl.accessToken = 'pk.eyJ1IjoicHJpZGVsaW5rIiwiYSI6ImNtMXhmdG1tdzB2Y20yanM1cXk1Mnc5MTkifQ.Zg_IyPxoqhldr72RyPiJZQ';

    const map = new mapboxgl.Map({
        container: 'map', // Your map container
        style: 'mapbox://styles/mapbox/light-v10',
        center: [-1.2965, 52.6282], // Default position in [longitude, latitude]
        zoom: 12
    });

    // Get user's current location
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            (position) => {
                const userLongitude = position.coords.longitude;
                const userLatitude = position.coords.latitude;
                console.log(userLongitude + "," + userLatitude)
                // Center the map on the user's current location
                map.setCenter([userLongitude, userLatitude]);
                map.setZoom(12); // Set zoom level as needed

                // Optional: Add a marker for the user's location
                new mapboxgl.Marker()
                    .setLngLat([userLongitude, userLatitude])
                    .setPopup(new mapboxgl.Popup().setHTML('<h4>You are here!</h4>')) // Add popup if needed
                    .addTo(map);
            },
            (error) => {
                console.error('Error getting location', error);
                // Optionally handle errors (e.g., permission denied)
            }
        );
    } else {
        console.log('Geolocation is not supported by this browser.');
    }

    const heatmapData = [
        [-1.2965, 52.6282, 0.7], // City center
        [-1.2950, 52.6315, 1.0], // Near Norwich Cathedral
        [-1.3060, 52.6282, 0.8], // Near the University of East Anglia
        [-1.2955, 52.6290, 0.6], // Near Norwich Market
        [-1.2975, 52.6350, 0.5], // Near Eaton Park
        [-1.2972, 52.6243, 0.4], // Near Mousehold Heath
        [-1.2941, 52.6321, 0.3], // Near Chapelfield Gardens
        [-1.2954, 52.6314, 0.9], // Near The Forum
        [-1.3032, 52.6315, 0.5], // Near Whittlingham Country Park
        [-1.3055, 52.6222, 0.2], // Near Earlham Cemetery
        [-1.2958, 52.6340, 0.8], // Near Norwich Train Station
        [-1.2963, 52.6285, 0.6], // Near the Arts Centre
        [-1.2905, 52.6300, 0.4]  // Near St. Stephen's Street
    ];

    // Create GeoJSON data structure
    const geojson = {
        type: 'FeatureCollection',
        features: heatmapData.map(point => ({
            type: 'Feature',
            geometry: {
                type: 'Point',
                coordinates: [point[0], point[1]],
            },
            properties: {
                intensity: point[2] // Use normalized intensity
            }
        }))
    };

    // Load the map
    map.on('load', function () {
        // Add a new source for the heatmap
        map.addSource('heatmap-data', {
            type: 'geojson',
            data: geojson,
        });

        // Add the heatmap layer
        map.addLayer({
            id: 'heatmap',
            type: 'heatmap',
            source: 'heatmap-data',
            maxzoom: 15,
            paint: {
                'heatmap-weight': [
                    'interpolate',
                    ['linear'],
                    ['get', 'intensity'],
                    0, 0,
                    1, 1
                ],
                'heatmap-color': [
                    'interpolate',
                    ['linear'],
                    ['heatmap-density'],
                    0, 'rgba(33,102,172,0)',
                    0.2, 'rgba(103,169,207,0.5)',
                    0.4, 'rgba(209,229,240,0.8)',
                    0.6, 'rgba(253,219,199,0.9)',
                    1, 'rgba(239,138,98,1)'
                ],
                'heatmap-radius': {
                    'base': 2,
                    'stops': [
                        [11, 15],
                        [15, 20]
                    ]
                },
                'heatmap-opacity': 0.8
            }
        });
    });
}
