
$(document).ready(
    $("#datepicker").datepicker({
        dateFormat: "dd/mm/yy",
        showAnim: "fold",
        changeMonth: true,
        changeYear: true
    })
)


function MunicipioGetByIdEstado() {
    let idEstado = $('#ddlEstado').val() // Alojar el Id del estado seleccionado
    //console.log(idEstado)
    $.ajax({
        url: "" + UrlEstados + idEstado + "", //mandar un unicio dato cadena interpolada
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

function ValidarImagen() { //validacion para saber si el usuario ingreso una imagen exclusivamente
    let imagen = $('#imagenInput')
    let nombreImagen = imagen[0].files[0].name //accede a la propiedad para el nombre del archivo
    let imagenSize = imagen[0].files[0].size
    var sizeKileByte = parseInt(imagenSize / 1024)
    console.log(sizeKileByte)

    //let extensionImg = nombreImagen.split(".")
    let extension = nombreImagen.split(".").pop() //elimina el primer arreglo

    //var extension = extensionImg[1]
    const validImagenExtension = ['jpeg', 'png', 'jpg', 'webp']; //arreglo para la validacion con el tipo de dato seleccionado
    banderaImg = false;

    for (var i = 0; i <= validImagenExtension.length; i++) {
        if (extension == validImagenExtension[i]) {
            banderaImg = true
        }
    }

    if (!banderaImg) {
        console.log("es otro archivo")
        alert(`Esta no es una imagen, solo formatos ${validImagenExtension}`)
        $('#imagenInput').val('')
        $('#fotoPerfil').attr('src', 'https://img.freepik.com/vector-gratis/avatar-personaje-empresario-aislado_24877-60111.jpg?t=st=1740679301~exp=1740682901~hmac=8d0df42d89f7a73d080b188cd7449336b9e10547b4c3751fa72055ae09087bf3&w=740');
    } else if (sizeKileByte > 2000) {
        alert("La imagen es muy pesada, debe ser de menos de 2MB")
        $('#imagenInput').val('')
        $('#fotoPerfil').attr('src', 'https://img.freepik.com/vector-gratis/avatar-personaje-empresario-aislado_24877-60111.jpg?t=st=1740679301~exp=1740682901~hmac=8d0df42d89f7a73d080b188cd7449336b9e10547b4c3751fa72055ae09087bf3&w=740');
        return false;
    } else {
        console.log("es una imagen")
        //console.log(`es una imagen, solo formatos ${validImagenExtension}`)
        const [file] = imagen[0].files
        if (file) {
            fotoPerfil.src = URL.createObjectURL(file)
            //VisualizarImagen(imagenVisual)
        }
    }
}

function validarCampo(eventoPress) {  //Recibe el evento del onkeypress
    var keypress = String.fromCharCode(eventoPress.which) //conversion del codigo de la tecla presionada para ser string
    var inputField = eventoPress.target //prop target para conocer en donde se encuentra mi input o cual es
    var errorMessage = inputField.parentNode.querySelector('.notificacion') //recupera la etiqueta que esta asociada con el input donde se hace el evento (dentro del div)
    console.log(keypress)
    errorMessage.textContent = '' //limpiar el span

    if ((/[^a-zA-Z\s]+/.test(keypress))) { //verifica si lo ingresado concuerda con la expresion regular
        //console.log("no es letra")
        eventoPress.preventDefault()
        inputField.style.borderWidth = "3px";
        inputField.style.borderColor = 'red'
        errorMessage.textContent = 'solo ingresar letras'
    } else {
        inputField.style.borderWidth = "3px";
        inputField.style.borderColor = 'green'

        //console.log("es letra")

    }
}

function validarNumeros(eventoPress) {
    var entrada = String.fromCharCode(eventoPress.which)
    var inputField = eventoPress.target
    var errorMessage = inputField.parentNode.querySelector('.notificacion')
    errorMessage.textContent = ''

    if (!(/^[0-9]{1,10}$/.test(entrada))) {
        eventoPress.preventDefault()
        inputField.style.borderWidth = "3px";
        inputField.style.borderColor = 'red'
        errorMessage.textContent = 'Solo ingresa numeros'
    } else {
        inputField.style.borderWidth = "3px";
        inputField.style.borderColor = 'green'
    }
}

function cambiarColor(eventoPress) {
    eventoPress.target.style.borderColor = 'green'
    eventoPress.target.style.borderWidth = '3px'
}

function validarEmail(evento) {
    var input = evento.target
    var emailInput = evento.target.value //nos trae el valor ingresado en el input
    //console.log(emailInput)
    var errorMessage = evento.target.parentNode.querySelector('.notificacion') // recupera el span asociado
    //console.log(errorMessage)
    errorMessage.textContent = ''

    if (!(/^\w+@@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/.test(emailInput))) {
        evento.preventDefault()
        input.style.borderWidth = "3px";
        input.style.borderColor = 'red'
        errorMessage.textContent = 'Email no valido'
    } else {
        input.style.borderWidth = "3px";
        input.style.borderColor = 'green'
    }
}

function validarInput(input) {
    var curp = input.value.toUpperCase(),
        resultado = document.getElementById("resultado"),
        valido = "Curp no válido"

    if (curpValida(curp)) {
        valido = "Curp válido"
        resultado.classList.add("ok")
        input.style.borderWidth = "3px";
        input.style.borderColor = 'green'
    } else {
        resultado.classList.remove("ok")
        input.style.borderWidth = "3px";
        input.style.borderColor = 'red'
    }

    resultado.innerText = valido
}


function curpValida(curp) {
    var re =
        /^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0\d|1[0-2])(?:[0-2]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$/,
        validado = curp.match(re)

    if (!validado)
        return false

    //Validar que coincida el dígito verificador
    function digitoVerificador(curp17) {
        //Fuente https://consultas.curp.gob.mx/CurpSP/
        var diccionario = "0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZ",
            lngSuma = 0.0,
            lngDigito = 0.0
        for (var i = 0; i < 17; i++)
            lngSuma = lngSuma + diccionario.indexOf(curp17.charAt(i)) * (18 - i)
        lngDigito = 10 - (lngSuma % 10)
        if (lngDigito == 10) return 0
        return lngDigito
    }
    if (validado[2] != digitoVerificador(validado[1])) return false

    return true

}

function userNameValid(eventoPress) {
    var letra = String.fromCharCode(eventoPress.which)
    var inputError = eventoPress.target
    var errorMessage = inputError.parentNode.querySelector('.notificacion')
    errorMessage.textContent = ''

    if (!(/^[a-zA-Z0-9]+$/.test(letra))) {
        eventoPress.preventDefault()
        inputError.style.borderWidth = "3px";
        inputError.style.borderColor = 'red'
        errorMessage.textContent = 'Solo numeros y letras'
    } else {
        inputError.style.borderWidth = "3px";
        inputError.style.borderColor = 'green'
    }
}

function validarPassword(evento) {
    var input = evento.target
    var passwordInput = input.value
    var errorMessage = input.parentNode.querySelector('.notificacion') // recupera el span asociado
    //console.log(errorMessage)
    errorMessage.textContent = ''

    if (!(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/.test(passwordInput))) {
        evento.preventDefault()
        input.style.borderWidth = "3px";
        input.style.borderColor = 'red'
        errorMessage.textContent = 'contraseña invalida: debe contener  + 8 caracteres, 1 caracter mayuscula, un simbolo y numeros '
    } else {
        input.style.borderWidth = "3px";
        input.style.borderColor = 'green'
    }
}

function contraseñasIguales(inputHelper) {
    var contraseñaHelper = $('#inptContraseña').val()
    var contraseñaValida = $('#inptValidar').val()
    $('.notificacion')[12].textContent = ''

    if (contraseñaHelper != contraseñaValida) {
        inputHelper.style.borderWidth = "3px";
        inputHelper.style.borderColor = 'red'
        $('.notificacion')[12].textContent = 'las contraseñas no coinciden favor de revisar'
    } else {
        inputHelper.style.borderWidth = "3px";
        inputHelper.style.borderColor = 'green'
    }
}

function campoVacio(evento) {
    var input = evento.target.value
    var errorMessage = evento.target.parentNode.querySelector('.notificacion')
    if (input == '') {
        evento.target.style.borderColor = ''
        errorMessage.textContent = ''
    } else {
        console.log("")
    }
}

function validarFormulario(evento) {
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
    console.log(evento)
    if (inptNombre == '' || inptApellidoPaterno == '' || inptApellidoMaterno == '' || inptTelefono == '' || inptFecha == '' || inptCelular == '' || inptCurp == '' ||
        inptCalle == '' || inptnExterior == '' || inptnInterior == '' || inptEmail == '' || inptUserName == '' || inptPassword == '' || inptConfirmPassword == '') {
        evento.preventDefault()
        alert("No se puede mandar el formulario, todos los campos deben de estar llenos")
    } else {
        alert("Formulario enviado correctamente")
    }
}
/* function VisualizarImagen(input) {
     console.log(input)
     if (input.files) { //si hay archivos
         var reader = new FileReader() // objeto para leer archivo de blob o file
         reader.onload = function (elemento) { //si el objeto se a leido correctamente hace esa funcion
             $('#fotoPerfil').attr('src', elemento.target.result)
             console.log(elemento)
         }
         reader.readAsDataURL(input.files)
     }
 }

 function ValidarImagen() {
     let imagen = $('#imagenInput')
     let nombreImagen = imagen[0].files[0].name
     let extension = nombreImagen.split(".").pop().toLowerCase() //elimina el primer arreglo

     $('#imagenInput').val('')
     $('#fotoPerfil').empty()

     console.log(extension)
     const validImagenExtension = ['jpeg', 'png', 'jpg', 'webp'];

     banderaImg = false;

     for (var i = 0; i <= validImagenExtension.length; i++) {
         if (extension == validImagenExtension[i]) {
             banderaImg = true
         }
     }

     if (!banderaImg) {
         console.log("es otro archivo")
         alert(`No es una imagen valida, solo extensiones ${validImagenExtension}`)
         $('#imagenInput').val('')
         $('#fotoPerfil').empty()
     } else {
         console.log("es una imagen")
         //VisualizarImagen(imagenVisual)
     }
 }*/



