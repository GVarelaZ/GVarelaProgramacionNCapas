using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Result //Ayuda a que si todo esta bien o hubo un error, para 
        //mostrarlo al usuario
    {
        public bool Correct { get; set; } // TRUE = proceso hecho correctamente
                                          // FALSE = hubo error
        public string ErrorMessage { get; set; } //Mensaje de error especifico
        public Exception ex { get; set; } //Trae todo el error a detalle
        public object Object { get; set; } // muestra solo 1 registro
        public List<object> Objects { get; set; } //Muestra TODOS los registros
    }
}
