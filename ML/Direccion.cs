﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Direccion
    {
        public int IdDireccion { get; set; }
        [DisplayName("Nombre de la calle:")]
        [Required(ErrorMessage ="La calle es obligatorio")]
        [RegularExpression(@"[a-zA-Z\s]+", ErrorMessage = "Solo se aceptan letras")]
        public string Calle { get; set; }
        [DisplayName("Numero interior:")]
        [Required(ErrorMessage ="Favor de digitar el número interior")]
        [RegularExpression(@"[0-9]{1,10}$", ErrorMessage = "Solo ingresar números")]
        public string NumeroInterior { get; set; }
        [DisplayName("Numero exterior:")]
        [Required(ErrorMessage ="Favor de digital el número exterior")]
        [RegularExpression(@"[0-9]{1,10}$", ErrorMessage = "Solo ingresar números")]
        public string NumeroExterior { get; set; }
        public Colonia Colonia { get; set; }
        public Usuario Usuario { get; set; }
    }
}
