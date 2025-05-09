flatpickr("#idCalendario", {
        enableTime: true,
        dateFormat: "d/m/Y H:i",
        time_24hr: true
});

// Función para alternar entre las vistas
function toggleView(view) {
    if (view === 'formPresencial') {
        document.getElementById('formularioPresencial').style.display = 'block';
        document.getElementById('formularioRemoto').style.display = 'none';
        document.getElementById('btnPresencial').classList.add('btn-active');
        document.getElementById('btnRemoto').classList.remove('btn-active');
    } else {
        document.getElementById('formularioPresencial').style.display = 'none';
        document.getElementById('formularioRemoto').style.display = 'block';
        document.getElementById('btnPresencial').classList.add('btn-active');
        document.getElementById('btnRemoto').classList.remove('btn-active');
    }
}

function bloquearSelect() {
    var defaultValue = "1"; // value you want to select

    // Set default selected
    $('#DLLCita').val(defaultValue);

    // Disable all options except the default
    $('#DLLCita option').each(function () {
        if ($(this).val() !== defaultValue) {
            $(this).attr('disabled', 'disabled');
        }
    });
};