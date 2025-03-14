using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Municipio
    {
        [Required(ErrorMessage = "Selecciona un municipio")]
        public int IdMunicipio { get; set; }
        [DisplayName("Municipio:")]
        public string Nombre { get; set; }
        public Estado Estado { get; set; }
        public List<object> Municipios { get; set; }

    }
}
