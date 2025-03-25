$(document).ready(function () {
    GetAll()
    console.log($('#idUsuario').val())
})

function GetAll() {
    console.log("Imprime tabla")
    var tabla = $('#tableGetAll')
    tabla.empty();

    $.ajax({
        url: rutaGetAll,
        type: "GET",
        dataType: "JSON",
        success: function (result) {
            console.log(result)
            if (result.Correct) {

                var usuarios = result.Objects
                var contador = 1
                var TBody = $('#tableGetAll')
                $.each(usuarios, function (i, usuario) {
                    let imagen = usuario.imagenJS == "" ? "https://img.freepik.com/vector-gratis/avatar-personaje-empresario-aislado_24877-60111.jpg?t=st=1740679301~exp=1740682901~hmac=8d0df42d89f7a73d080b188cd7449336b9e10547b4c3751fa72055ae09087bf3&w=740" : `"data: image/*;base64,${usuario.imagenJS}"`
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
                                        <button class="btn btn-warning text-lg-center" onclick="formulario(${usuario.idUsuario})" role="button">
                                            <i class="bi bi-pencil-fill"></i>
                                        </a>
                                        <button class="btn btn-danger" onclick="verificarConfirm(event,${usuario.idUsuario})" role="button">
                                            <i class="bi bi-trash3-fill"></i>
                                        </button>
                                    </td>
                                </tr>`
                    contador++

                    TBody.append(registro)

                })
            } else {
                alert(result.ErrorMessage)
            }
        },
        error: function (xhr) {
            console.log(xhr)
        }

    })
}

function EnviarFormulario() {

    var json = ObtenerValores()

    $.ajax({
        url: rutaForm,
        type: "POST",
        dataType: "JSON",
        data: JSON.stringify(json),
        contentType: "application/json; charset=UTF-8",
        success: function (result) {
            alert("Se han realizado los cambios exitosamente")
            window.location.reload()
        },
        error: function (xhr) {
            console.log(xhr)
        }
    })
}

function selectAddUpdate(idUsuario) { //mostrar formulario lleno o vacio

    $.ajax({
        url: `${rutaForm}?idUsuario=${idUsuario}`,
        type: "GET",
        dataType: "JSON",
        success: function (usuario) {
            if (idUsuario != 0) {
                $('#idUsuario').val(usuario.idUsuario)
                $('#nombre').val(usuario.Nombre)
                $('#apellidoPaterno').val(usuario.ApellidoPaterno)
                $('#apellidoMaterno').val(usuario.ApellidoMaterno)
                if (usuario.Sexo == "H ") {
                    $('#SexoH').prop('checked', true)
                } else {
                    $('#SexoM').prop('checked', true)
                }
                $('#telefono').val(usuario.Telefono)
                $('#datepicker').val(usuario.FechaNacimiento)
                $('#celular').val(usuario.Celular)
                //$('#Rol').val(usuario.Rol.Nombre).change()
                $(`#Rol option[value='${usuario.Rol.IdRol}']`).prop('selected', true)
                $('#curp_input').val(usuario.Curp)
                $('#calle').val(usuario.Direccion.Calle)
                $('#numeroExterior').val(usuario.Direccion.NumeroExterior)
                $('#numeroInterior').val(usuario.Direccion.NumeroInterior)
                $(`#ddlEstado option[value='${usuario.Direccion.Colonia.Municipio.Estado.IdEstado}']`).prop('selected', true)

                $('#ddlMunicipio').empty();
                $('#ddlMunicipio').append("<option value=0>Selecciona un municìpio</option>")
                $('#ddlColonia').empty();
                $('#ddlColonia').append("<option value=0>Selecciona una colonia</option>")
                let ddlMunicipio = $('#ddlMunicipio') //Asignamos la variable del ddl a llenar

                $.each(usuario.Direccion.Colonia.Municipio.Municipios, function (i, valor) {
                    let opcion = `<option value=${valor.IdMunicipio}> ${valor.Nombre} </option>` //Llenamos la etiqueta select con opcion
                    ddlMunicipio.append(opcion) //se insertan los opcion
                })
                $(`#ddlMunicipio option[value='${usuario.Direccion.Colonia.Municipio.IdMunicipio}']`).prop('selected', true)

                $('#ddlColonia').empty();
                $('#ddlColonia').append("<option value=0>Selecciona una colonia</option>")
                let ddlColonia = $('#ddlColonia')
                $.each(usuario.Direccion.Colonia.Colonias, function (i, colonia) {
                    let opcion = `<option value=${colonia.IdColonia}>${colonia.Nombre}</option>`
                    //console.log(opcion)
                    ddlColonia.append(opcion)
                })
                $(`#ddlColonia option[value='${usuario.Direccion.Colonia.IdColonia}']`).prop('selected', true)
                $('#emailInput').val(usuario.Email)
                $('#UserName').val(usuario.UserName)
                $('#inptContraseña').val(usuario.Password)
                $('#inptValidar').val(usuario.Password)
                let imagen = usuario.imagenJS == "" ? "https://img.freepik.com/vector-gratis/avatar-personaje-empresario-aislado_24877-60111.jpg?t=st=1740679301~exp=1740682901~hmac=8d0df42d89f7a73d080b188cd7449336b9e10547b4c3751fa72055ae09087bf3&w=740" : `data:image/*;base64,${usuario.imagenJS}`
                $("#fotoPerfil").attr("src", `${imagen}`)
                $('#imagen').val(usuario.imagenJS)
                $('#idDireccion').val(usuario.Direccion.IdDireccion)
            }
        },
        error: function (xhr) {
            console.log(xhr)
        }
    })
}
function verificarConfirm(e, idUsuario) {
    if (!confirm("¿Seguro que deseas eliminar el registro?")) {
        e.preventDefault(); // si da cancelar, no hace nada
    } else {
        //si da aceptar
        DeleteJS(idUsuario)
        //console.log(idUsuario)
    }
}
function DeleteJS(IdUsuario) {
    //console.log(IdUsuario)
    $.ajax({
        url: `${rutaDelete}?idUsuario=${IdUsuario}`,
        type: "GET",
        dataType: "JSON",
        success: function (evento) {
            console.log(evento)
            alert(evento.ErrorMessage)
            GetAll()
        },
        error: function (xhr) {
            console.log(xhr)

        }
    })

}
function formulario(idUsuario) {
    LimpiarModal()
    DDLRol()
    DDLEstado()
    selectAddUpdate(idUsuario)
    MostrarModal()
}
function MostrarModal() {
    $('#Modal').modal("show")
}
function LimpiarModal() {
    let inputs = $('.inpt')
    let selects = $('.select')
    console.log(inputs)
    $.each(inputs, function (i, input) {
        input.value = ""
        input.style.borderColor = ""
        input.checked = false
    })
    $.each(selects, function (i, select) {
        console.log(select.options)
        select.style.borderColor = ""
        var length = select.options.length;
        for (i = length - 1; i >= 0; i--) {
            select.options[i] = null;
        }
    })
    $('#fotoPerfil').attr("src", "https://img.freepik.com/vector-gratis/avatar-personaje-empresario-aislado_24877-60111.jpg?t=st=1740679301~exp=1740682901~hmac=8d0df42d89f7a73d080b188cd7449336b9e10547b4c3751fa72055ae09087bf3&w=740")
}
function DDLRol() {
    let ddlRol = $('#Rol')
    //console.log(ddlRol)

    $.ajax({
        url: rutaDDLRol,
        type: "GET",
        dataType: "JSON",
        success: function (result) {
            //console.log(result)
            if (result.Correct) {

                $('#Rol').append("<option value=0>Selecciona un rol</option>")
                $.each(result.Objects, function (i, valor) {
                    //console.log(i)
                    //console.log(valor)
                    let opcion = `<option value=${valor.IdRol}> ${valor.Nombre} </option>` //Llenamos la etiqueta select con opcion
                    ddlRol.append(opcion) //se insertan los opcion
                })
            }
        },
        error: function (xhr) {
            cons0ole.log(xhr)
        }
    })
}
function DDLEstado() {
    let ddlEstado = $('#ddlEstado')
    //console.log(ddlRol)

    $.ajax({
        url: rutaDDLEstado,
        type: "GET",
        dataType: "JSON",
        success: function (result) {
            //console.log(result)
            if (result.Correct) {

                $('#ddlEstado').append("<option value=0>Selecciona un estado</option>")
                $('#ddlMunicipio').append("<option value=0>Selecciona un municipio</option>")
                $('#ddlColonia').append("<option value=0>Selecciona una colonia</option>")
                $.each(result.Objects, function (i, valor) {
                    //console.log(i)
                    //console.log(valor)
                    let opcion = `<option value=${valor.IdEstado}> ${valor.Nombre} </option>` //Llenamos la etiqueta select con opcion
                    ddlEstado.append(opcion) //se insertan los opcion
                })
            }
        },
        error: function (xhr) {
            cons0ole.log(xhr)
        }
    })
}

function MunicipioGetByIdEstado() {
    let idEstado = $('#ddlEstado').val() // Alojar el Id del estado seleccionado
    //console.log(idEstado)
    $.ajax({
        url: UrlEstados + idEstado, //mandar un unicio dato cadena interpolada
        type: "GET",
        dataType: "JSON",
        //data : Modelo enviar
        success: function (result) {
            //console.log(result) //Envia el result que viene del BL
            if (result.Correct) {
                //console.log(result.Correct)
                $('#ddlMunicipio').empty();
                $('#ddlMunicipio').append("<option value=0>Selecciona un municìpio</option>")
                $('#ddlColonia').empty();
                $('#ddlColonia').append("<option value=0>Selecciona una colonia</option>")

                let ddlMunicipio = $('#ddlMunicipio') //Asignamos la variable del ddl a llenar

                $.each(result.Objects, function (i, valor) {
                    //console.log(i)
                    //console.log(valor)
                    let opcion = `<option value=${valor.IdMunicipio}> ${valor.Nombre} </option>` //Llenamos la etiqueta select con opcion
                    ddlMunicipio.append(opcion) //se insertan los opcion
                })
            } else {

            }
        },
        error: function (xhr) {
            console.log(xhr)
        }
    })
}

function ColoniaGetByIdMunicipio() {
    let ddlMunicipio = $('#ddlMunicipio').val()
    //console.log(ddlMunicipio)

    $.ajax({
        url: "" + UrlMunicipio + ddlMunicipio + "",
        type: "GET",
        dataType: "JSON",
        success: function (result) {
            //console.log(result)
            if (result.Correct) {
                $('#ddlColonia').empty();
                $('#ddlColonia').append("<option value=0>Selecciona una colonia</option>")
                let ddlColonia = $('#ddlColonia')

                $.each(result.Objects, function (i, colonia) {
                    let opcion = `<option value=${colonia.IdColonia}>${colonia.Nombre}</option>`
                    //console.log(opcion)
                    ddlColonia.append(opcion)
                })
            }
        },
        error: function (xhr) {
            console.log(xhr)
        }
    })
}

function ValidarFormularioJS() {
    var inptNombre = $('#nombre').val()
    var inptApellidoPaterno = $('#apellidoPaterno').val()
    var inptApellidoMaterno = $('#apellidoMaterno').val()
    var inptTelefono = $('#Telefono').val()
    var inptFecha = $('#datepicker').val()
    var inptCelular = $('#Celular').val()
    var inptCurp = $('#curp_input').val()
    var inptCalle = $('#Calle').val()
    var inptnExterior = $('#numeroExterior').val()
    var inptnInterior = $('#numeroInterior').val()
    var inptEmail = $('#emailInput').val()
    var inptUserName = $('#UserName').val()
    var inptPassword = $('#inptContraseña').val()
    var inptConfirmPassword = $('#inptValidar').val()
    if (inptNombre == '' || inptApellidoPaterno == '' || inptApellidoMaterno == '' || inptTelefono == '' || inptFecha == '' || inptCelular == '' || inptCurp == '' ||
        inptCalle == '' || inptnExterior == '' || inptnInterior == '' || inptEmail == '' || inptUserName == '' || inptPassword == '' || inptConfirmPassword == '') {
        alert("No se puede mandar el formulario, todos los campos deben de estar llenos")
    } else {
        alert("Formulario enviado correctamente")
        EnviarFormulario()
    }
}

function ObtenerValores() {
    var sexo
    var imagenMandar
    var selector = $('#SexoH')[0].checked
    var imagen = $('#imagen').val()
    if (selector) {
        sexo = "H"
    } else {
        sexo = "M"
    }
    console.log($('#imagenInput')[0].files)
    imgInpt = $('#imagenInput')[0]
    if (imgInpt.files.length > 0) { //si hay archivos
        var reader = new FileReader() // objeto para leer archivo de blob o file
        reader.onload = function (elemento) { //si el objeto se a leido correctamente hace esa funcion
            console.log(elemento.target.result)
        }
        reader.readAsDataURL(imgInpt.files[0])
    }else if (imagen != "") {
        imagenMandar = imagen
    }

    var json = {
        "idUsuario": $('#idUsuario').val(),
        "nombre": $('#nombre').val(),
        "apellidoPaterno": $('#apellidoPaterno').val(),
        "apellidoMaterno": $('#apellidoMaterno').val(),
        "telefono": $('#telefono').val(),
        "userName": $('#UserName').val(),
        "password": $('#inptContraseña').val(),
        "fechaNacimiento": $('#datepicker').val(),
        "sexo": sexo,
        "celular": $('#celular').val(),
        "curp": $('#curp_input').val(),
        "email": $('#emailInput').val(),
        "rol": {
            "idRol": $('#Rol').val(),
        },
        "direccion": {
            "idDireccion": $('#idDireccion').val(),
            "calle": $('#calle').val(),
            "numeroInterior": $('#numeroInterior').val(),
            "numeroExterior": $('#numeroExterior').val(),
            "colonia": {
                "idColonia": $('#ddlColonia').val(),
                "municipio": {
                    "idMunicipio": $('#ddlMunicipio').val(),
                    "estado": {
                        "idEstado": $('#ddlEstado').val(),
                    }
                }

            }
        },
        "imagenJS": imagenMandar
    }

    return json
    //console.log(json)
}
