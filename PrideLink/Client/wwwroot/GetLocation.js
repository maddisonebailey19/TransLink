// GetLocation.js
window.getLocation = function () {
    return new Promise((resolve, reject) => {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                function (position) {
                    // Resolve the promise with latitude and longitude
                    resolve({ latitude: position.coords.latitude, longitude: position.coords.longitude });
                },
                function (error) {
                    console.error("Geolocation error: " + error.message);
                    reject(error);
                }
            );
        } else {
            console.error("Geolocation is not supported by this browser.");
            reject(new Error("Geolocation not supported"));
        }
    });
}
