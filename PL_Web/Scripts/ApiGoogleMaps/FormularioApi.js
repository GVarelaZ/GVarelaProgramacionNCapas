
var marker = null;
var markerEmpresa = null;
var IdEmpresa = $('#IdEmpresa').val()
var latitud = $('#Latitud').val()
var longitud = $('#Longitud').val()
var Inptlatitud = $('#Latitud')
var InptlatitudHiden = $('#LatitudHiden')
var InptlongitudHiden = $('#LongitudHiden')
var Inptlongitud = $('#Longitud')
$(document).ready(function () {
    iniciarMap()
    //obtenerValores()
})

async function iniciarMap() {
    const center = { lat: 19.4324709, lng: -99.1329094 };
    //console.log(IdEmpresa)
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

        //console.log('Latitude: ' + lat + ', Longitude: ' + lng);
        Inptlatitud.val(lat)
        InptlatitudHiden.val(lat)
        Inptlongitud.val(lng) 
        InptlongitudHiden.val(lng)
    });
}
