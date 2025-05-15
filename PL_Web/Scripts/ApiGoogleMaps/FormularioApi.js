
var marker = null;
var markerEmpresa = null;
$(document).ready(function () {
    iniciarMap()
    //obtenerValores()
})

async function iniciarMap() {
    const center = { lat: 19.4324709, lng: -99.1329094 };

    var IdEmpresa = $('#IdEmpresa').val()
    var latitud = $('#Latitud').val()
    var longitud = $('#Longitud').val()
    var Inptlatitud = $('#Latitud')
    var Inptlongitud = $('#Longitud')
    console.log(IdEmpresa)
    //@ts-ignore
    var { Map } = await google.maps.importLibrary("maps");
    var { AdvancedMarkerElement } = await google.maps.importLibrary("marker");

    // The map, centered at Uluru
    const map = new Map(document.getElementById("mapa"), {
        zoom: 12,
        center: center,
        mapId: "DEMO_MAP_ID",
    });

    if (IdEmpresa != 0) {

        var positionEmpresa = { lat: parseFloat(latitud), lng: parseFloat(longitud) };

        markerEmpresa = new google.maps.marker.AdvancedMarkerElement({
            map,
            position: positionEmpresa,
        });
    }

    google.maps.event.addListener(map, 'click', function (event) {

        if (markerEmpresa) {
            markerEmpresa.setMap(null)
        }
        
        var lat = event.latLng.lat();
        var lng = event.latLng.lng();

        if (marker) {
            marker.setMap(null);
        }

        marker = new google.maps.marker.AdvancedMarkerElement({
            position: { lat: lat, lng: lng },
            map: map,
            title: 'Selected Location'
        });

        // Display the coordinates (or use them for other purposes)
        console.log('Latitude: ' + lat + ', Longitude: ' + lng);
        Inptlatitud.val(lat)
        Inptlongitud.val(lng) 
    });
}
