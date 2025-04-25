using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Candidato
    {
        public int IdCandidato { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [RegularExpression(@"[a-zA-Z\s]+", ErrorMessage = "Solo se aceptan letras")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        [RegularExpression(@"[a-zA-Z\s]+", ErrorMessage = "Solo se aceptan letras")]
        public string ApellidoPaterno { get; set; }

        [Required(ErrorMessage = "El apellido materno es obligatorio")]
        [RegularExpression(@"[a-zA-Z\s]+", ErrorMessage = "Solo se aceptan letras")]
        public string ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "La edad es obligatorio")]
        [RegularExpression(@"[0-9]{1,10}$", ErrorMessage = "Solo ingresar números")]
        public string Edad { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [RegularExpression(@"\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b", ErrorMessage = "No es un email válido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El telefono es obligatorio")]
        [RegularExpression(@"[0-9]{1,10}$", ErrorMessage = "Solo ingresar números")]
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public byte[] Foto { get; set; }
        public byte[] Curriculum { get; set; }
        public Universidad universidad { get; set; }
        public Carrera carrera { get; set; }
        public Promedio promedio { get; set; }
        public BolsaTrabajo bolsaTrabajo { get; set; }
        public Vacante vacante { get; set; }
        public Cita cita { get; set; }
        public List<object> Candidatos { get; set; }
    }
}
