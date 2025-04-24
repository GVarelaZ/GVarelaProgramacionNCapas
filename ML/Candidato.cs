using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Candidato
    {
        public int IdCandidato { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Edad { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public byte[] Foto { get; set; }
        public byte[] Curriculum { get; set; }
        public Universidad universidad { get; set; }
        public Carrera carrera { get; set; }
        public Promedio promedio { get; set; }
        public BolsaTrabajo bolsaTrabajo { get; set; }
        public Vacante vacante { get; set; }
        public List<object> Candidatos { get; set; }
    }
}
