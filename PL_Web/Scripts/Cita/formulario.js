flatpickr("#idCalendario", {
    enableTime: true,
    dateFormat: "d/m/Y H:i",
    time_24hr: true,
    minDate: "today",
    "disable": [
        function (date) {
            // return true to disable
            return (date.getDay() === 0 || date.getDay() === 6);

        }
    ],
    "locale": {
        "firstDayOfWeek": 1 // start week on Monday
    },
    minTime: "10:00",
    maxTime: "16:00"
});

// Función para alternar entre las vistas
function toggleView(view) {
    if (view === 'formPresencial') {
        $('#formularioPresencial')[0].style.display = 'block';
        $('#formularioRemoto')[0].style.display = 'none';
    } else {
        $('#formularioPresencial')[0].style.display = 'none';
        $('#formularioRemoto')[0].style.display = 'block';
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