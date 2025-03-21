$(document).ready(
    GetAll()
)

function GetAll() {
    console.log("Imprime tabla")
    var tabla = $('#tableGetAll')

    $.ajax({
        url: rutaGetAll,
        type: "GET",
        dataType: "JSON",
        success: function (result) {
            var usuarios = result.Objects
            var contador = 1
            var TBody = $('#tableGetAll')
            $.each(usuarios, function (i, usuario){
                let imagen = usuario.Imagen == null ? "https://img.freepik.com/vector-gratis/avatar-personaje-empresario-aislado_24877-60111.jpg?t=st=1740679301~exp=1740682901~hmac=8d0df42d89f7a73d080b188cd7449336b9e10547b4c3751fa72055ae09087bf3&w=740" : ""
                let sexo = usuario.Sexo == "H " ? "bi bi-person-standing" : "bi bi-person-standing-dress"
                let color = usuario.Sexo == "H " ? "blue" : "deeppink"
                let estatus = usuario.Estatus == true ? "checked" : ""
                let registro = `<tr>
                                    <td class="panel" id="@contador"
                                        style=" text-align:center;">
                                        ${contador}
                                    </td>
                                    <td class="text-center">
                                        <div>
                                            <div style="margin-left: 4px">${usuario.Nombre} ${usuario.ApellidoPaterno} ${usuario.ApellidoMaterno}</div>
                                            
                                             <img id="fotoPerfil" class="avatar" src=${imagen}
                                                     width="60px" height="60px">
                                            <span class="badge text-bg-info">
                                                ${usuario.Rol.Nombre}
                                            </span>
                                            <br>
                                            ${usuario.Curp}

                                        </div>
                                    </td>
                                    <td class="text-center">
                                        <span class="badge text-bg-success">
                                            <i class="bi bi-telephone-fill"></i>
                                            ${usuario.Telefono}
                                        </span>
                                        <br>
                                        <span class="badge text-bg-primary">
                                            <i class="bi bi-telephone-fill"></i>
                                            ${usuario.Celular}
                                        </span>
                                        <br>
                                        <span class="badge text-bg-dark">
                                            <i class="bi bi-envelope-at-fill"></i>
                                            ${usuario.Email}
                                        </span>
                                    </td>
                                    <td class="text-center">${usuario.FechaNacimiento}</td>
                                    <td class="text-center">
                                            <div>
                                                <i style="color: ${color}; font-size: 2.5em;" class="${sexo}"></i>
                                            </div>
                                    </td>
                                    <td class="text-center">
                                        <div>
                                            calle ${usuario.Direccion.Calle}, Int.#${usuario.Direccion.NumeroInterior}, Ext.#${usuario.Direccion.NumeroExterior}, colonia ${usuario.Direccion.Colonia.Nombre}, C.P.${usuario.Direccion.Colonia.CodigoPostal},
                                            ${usuario.Direccion.Colonia.Municipio.Nombre}, ${usuario.Direccion.Colonia.Municipio.Estado.Nombre}
                                        </div>
                                    </td>
                                    <td class="text-center">
                                            <div class="form-check form-switch text-center">
                                                <input class="form-check-input" type="checkbox" id="flexSwitchCheckDefault" ${estatus}
                                                       onchange="CambioStatus(this,${usuario.idUsuario} )">
                                            </div>
                                        <a href="@Url.Action("Form","Usuario", new {IdUsuario = usuario.idUsuario, IdDireccion = usuario.Direccion.IdDireccion})"
                                           class="btn btn-warning text-lg-center" role="button">
                                            <i class="bi bi-pencil-fill"></i>
                                        </a>
                                        <a href="@Url.Action("Delete","Usuario", new {IdUsuario = usuario.idUsuario})"
                                           class="btn btn-danger" onclick="return confirm('Seguro que deseas eliminar el registro?')" role="button">
                                            <i class="bi bi-trash3-fill"></i>
                                        </a>
                                    </td>
                                </tr>`
                contador++

                TBody.append(registro)

            })    
        },
        error: function (xhr) {
            console.log(xhr)
        }

    })
}