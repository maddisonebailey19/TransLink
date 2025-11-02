// Mapbox helper that waits for style to be loaded before adding sources/layers.
// Keeps a pendingGeojson so updateHeatmap can be called anytime.
window._mapInstance = window._mapInstance || null;
window._pendingGeojson = null;

function initializeMap(jsonData) {
    console.log('initializeMap called');

    // create the map once
    if (!window._mapInstance) {
        mapboxgl.accessToken = 'pk.eyJ1IjoicHJpZGVsaW5rIiwiYSI6ImNtMXhmdG1tdzB2Y20yanM1cXk1Mnc5MTkifQ.Zg_IyPxoqhldr72RyPiJZQ';
        window._mapInstance = new mapboxgl.Map({
            container: 'map',
            style: 'mapbox://styles/mapbox/light-v10',
            center: [1.212912, 52.6282],
            zoom: 12
        });

        // on load create the source and layers (if pending data exists it will be applied)
        window._mapInstance.on('load', () => {
            console.log('map load event fired');
            // create empty source
            if (!window._mapInstance.getSource('heatmap-data')) {
                window._mapInstance.addSource('heatmap-data', { type: 'geojson', data: { type: 'FeatureCollection', features: [] } });
            }

            // add heatmap layer if not exists
            if (!window._mapInstance.getLayer('heatmap')) {
                window._mapInstance.addLayer({
                    id: 'heatmap',
                    type: 'heatmap',
                    source: 'heatmap-data',
                    maxzoom: 15,
                    paint: {
                        'heatmap-weight': ['interpolate', ['linear'], ['get', 'intensity'], 0, 0, 1, 1],
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
                        'heatmap-radius': ['interpolate', ['linear'], ['zoom'], 0, 2, 11, 15, 15, 20],
                        'heatmap-opacity': 0.8
                    }
                });
            }

            // add points layer for clicks
            if (!window._mapInstance.getLayer('points')) {
                window._mapInstance.addLayer({
                    id: 'points',
                    type: 'circle',
                    source: 'heatmap-data',
                    paint: {
                        'circle-radius': 6,
                        'circle-color': 'rgba(0,0,0,0.7)',
                        'circle-stroke-color': '#fff',
                        'circle-stroke-width': 1
                    }
                });

                window._mapInstance.on('click', 'points', (e) => {
                    const coords = e.features[0].geometry.coordinates.slice();
                    const props = e.features[0].properties || {};
                    new mapboxgl.Popup().setLngLat(coords).setHTML(`<div>${props.description || ''}</div>`).addTo(window._mapInstance);
                });
                window._mapInstance.on('mouseenter', 'points', () => { window._mapInstance.getCanvas().style.cursor = 'pointer'; });
                window._mapInstance.on('mouseleave', 'points', () => { window._mapInstance.getCanvas().style.cursor = ''; });
            }

            // If there's pending data (updateHeatmap was called before load), apply it now
            if (window._pendingGeojson) {
                try {
                    const src = window._mapInstance.getSource('heatmap-data');
                    if (src) src.setData(window._pendingGeojson);
                    window._pendingGeojson = null;
                } catch (err) {
                    console.error('Failed to set pending geojson on load:', err);
                }
            }
        });
    }

    // Always delegate to updateHeatmap which handles style/not-loaded logic
    updateHeatmap(jsonData);
}

function updateHeatmap(jsonData) {
    console.log('updateHeatmap called');

    let points = [];
    try {
        if (jsonData) points = JSON.parse(jsonData);
    } catch (err) {
        console.error('Failed to parse heatmap jsonData', err);
        points = [];
    }

    // normalize to GeoJSON features
    const features = (points || []).map(p => {
        if (Array.isArray(p)) {
            const lng = Number(p[0]);
            const lat = Number(p[1]);
            const intensity = Number(p[2] ?? 0);
            return {
                type: 'Feature',
                geometry: { type: 'Point', coordinates: [lng, lat] },
                properties: { intensity: Math.max(0, Math.min(1, intensity)), description: '' }
            };
        } else {
            const lng = Number(p.Longitude ?? p.longitude ?? p.Lng ?? p.lng);
            const lat = Number(p.Latitude ?? p.latitude ?? p.Lat ?? p.lat);
            const intensity = Number(p.Intensity ?? p.intensity ?? 0);
            return {
                type: 'Feature',
                geometry: { type: 'Point', coordinates: [lng, lat] },
                properties: { intensity: Math.max(0, Math.min(1, intensity)), description: p.Description ?? p.description ?? '' }
            };
        }
    }).filter(f => Number.isFinite(f.geometry.coordinates[0]) && Number.isFinite(f.geometry.coordinates[1]));

    const geojson = { type: 'FeatureCollection', features: features };
    console.log('Prepared GeoJSON features:', geojson.features.length);

    if (!window._mapInstance) {
        console.warn('Map instance not initialized yet - storing pending data and calling initializeMap');
        window._pendingGeojson = geojson;
        initializeMap(JSON.stringify(points));
        return;
    }

    // If style isn't loaded yet, store pending and wait for load handler
    if (!window._mapInstance.isStyleLoaded || !window._mapInstance.isStyleLoaded()) {
        console.warn('Map style not loaded yet - queuing geojson');
        window._pendingGeojson = geojson;
        return;
    }

    // style is loaded and source should exist (created in load handler), update or add as needed
    try {
        const src = window._mapInstance.getSource('heatmap-data');
        if (src) {
            src.setData(geojson);
        } else {
            window._mapInstance.addSource('heatmap-data', { type: 'geojson', data: geojson });
        }

        if (geojson.features.length) {
            const bounds = geojson.features.reduce((b, f) => b.extend(f.geometry.coordinates), new mapboxgl.LngLatBounds(geojson.features[0].geometry.coordinates, geojson.features[0].geometry.coordinates));
            window._mapInstance.fitBounds(bounds, { padding: 40, maxZoom: 14 });
        } else {
            console.log('No features - heatmap cleared');
        }

        window._mapInstance.resize();
    } catch (err) {
        console.error('Error updating heatmap source:', err);
        // fallback: queue pending and let load handler apply it
        window._pendingGeojson = geojson;
    }
}