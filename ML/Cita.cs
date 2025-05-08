using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Cita
    {
        public int IdCita { get; set; }
        public string FechaHora { get; set; }
        public string Url { get; set; }
        public Piso piso { get; set; }
        public EstatusCita estatusCita { get; set; }
    }
}
