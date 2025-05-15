let map;
$(document).ready(function () {
    /*console.log("hola")*/
    getAllEmpresas();
})

function getAllEmpresas() {
    var tabla = $('#tablaEmpresa')

    $.ajax({
        url: obtenerEmpresas,
        type: "GET",
        dateType: "JSON",
        success: function (result) {
            //console.log(result)
            initMap(result.Objects)
            $.each(result.Objects, function (index, empresa) {
                var columna = `<tr>
                                <td>${index + 1}</td>
                                <td>${empresa.Nombre}</td>
                                <td>
                                    <a class="btn btn-warning text-lg-center" href="${formuActualizar}${empresa.IdEmpresa}" role="button">
                                            <i class="bi bi-pencil-fill"></i></a>
                                    <a class="btn btn-danger" href="${eliminarEmpresa}${empresa.IdEmpresa}"
                                    onclick="return confirm('Seguro que deseas eliminar el registro?')" role="button">
                                        <i class="bi bi-trash3-fill"></i></a>
                                </td>
                               </tr>`

                tabla.append(columna)
            })


        },
        error: function (xhr) {

        }
    })
}
// Initialize and add the map


async function initMap(empresas) {


    const center = { lat: 19.4324709, lng: -99.1329094 };

    var { Map } = await google.maps.importLibrary("maps");
    var { AdvancedMarkerElement } = await google.maps.importLibrary("marker");


    map = new Map(document.getElementById("map"), {
        zoom: 12,
        center: center,
        mapId: "DEMO_MAP_ID",
    });
    $.each(empresas, function (i, empresa) {
        //console.log(empresa)
        var etiqueta = `<div>
                            <h5>${empresa.Nombre}</h5>
                            <h6>Latitud: ${empresa.Latitud}</h6>
                            <h6>Longitud: ${empresa.Longitud}</h6>
                        </div>`

        var position = { lat: parseFloat(empresa.Latitud), lng: parseFloat(empresa.Longitud) };

        const marker = new google.maps.marker.AdvancedMarkerElement({
            map,
            position: position,
            title: `${empresa.Nombre}`,
        });

        const infowindow = new google.maps.InfoWindow({
            content: etiqueta,
        });

        marker.addListener("click", () => {
            infowindow.open({
                anchor: marker,
                map,
            });
        });
    })


}
