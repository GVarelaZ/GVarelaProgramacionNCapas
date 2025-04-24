using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Promedio
    {
        public int IdPromedio { get; set; }

        //public int IdCandidato { get; set; }
        public string PromedioUniversidad { get; set; }
        public string PromedioPreparatoria { get; set; }
        public string PromedioSecundaria { get; set; }

        public List<object> Promedios { get; set; }
    }
}
