using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Colonia
    {
        [Required(ErrorMessage ="Selecciona una colonia")]
        public int IdColonia { get; set; }
        [DisplayName("Colonia:")]
        public string Nombre { get; set; }
        public string CodigoPostal { get; set; }
        public Municipio Municipio { get; set; }
        public List<object> Colonias { get; set; }
    }
}
