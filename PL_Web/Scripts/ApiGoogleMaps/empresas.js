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
                var columna = tablaBody(index, empresa)
                tabla.append(columna)
            })
        },
        error: function (xhr) {

        }
    })
}
async function initMap(empresas) {

    var { Map } = await google.maps.importLibrary("maps");
    var { AdvancedMarkerElement } = await google.maps.importLibrary("marker");
    var bounds = new google.maps.LatLngBounds();

    map = new Map(document.getElementById("map"), {
        mapId: "DEMO_MAP_ID",
    });
    $.each(empresas, function (i, empresa) {
        //console.log(empresa)
        var etiqueta = etiquetaMarcador(empresa)

        var position = { lat: parseFloat(empresa.Latitud), lng: parseFloat(empresa.Longitud) };
        bounds.extend(position);

        marcadores(empresa, position, etiqueta)

    })
    map.fitBounds(bounds);
}
function etiquetaMarcador(empresa) {
    return `<div>
                <h5>${empresa.Nombre}</h5>
                <h6>Latitud: ${empresa.Latitud}</h6>
                <h6>Longitud: ${empresa.Longitud}</h6>
            </div>`
}
function tablaBody(index, empresa) {
    return `<tr>
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
}
function marcadores(empresa, posicion, etiqueta) {
    const marker = new google.maps.marker.AdvancedMarkerElement({
        map,
        position: posicion,
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
}