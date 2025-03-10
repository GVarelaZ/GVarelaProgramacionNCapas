using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ML // ML = Model Layout
            // el model son como los campos para la base de datos
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Telefono { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string Celular { get; set; }
        public bool Estatus { get; set; }
        public string Curp { get; set; }
        public byte[] Imagen { get; set; }
        public string Email { get; set; }
        public Rol Rol { get; set; }
        public Direccion Direccion { get; set; }
        public List<object> Usuarios { get; set; }

    }
}
