using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ML // ML = Model Layout
             // el model son como los campos para la base de datos
{
    public class Usuario
    {
        //DataAnnotations
        public int idUsuario { get; set; }
        [DisplayName("Nombre(s):")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [RegularExpression(@"/[^a-zA-Z\s]+/", ErrorMessage = "Solo se aceptan letras")]
        public string Nombre { get; set; }
        [DisplayName("Apellido paterno:")]
        [Required(ErrorMessage = "El apellido materno es obligatorio")]
        [RegularExpression(@"/[^a-zA-Z\s]+/", ErrorMessage = "Solo se aceptan letras")]
        public string ApellidoPaterno { get; set; }
        [DisplayName("Apellido materno:")]
        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        [RegularExpression(@"/[^a-zA-Z\s]+/", ErrorMessage = "Solo se aceptan letras")]
        public string ApellidoMaterno { get; set; }
        [DisplayName("Telefono de casa:")]
        [Required(ErrorMessage = "El telefono es obligatorio")]
        [RegularExpression(@"/^[0-9]{1,10}$/", ErrorMessage = "Solo ingresar números")]
        public string Telefono { get; set; }
        [DisplayName("Nombre de usuario:")]
        [Required(ErrorMessage = "El nombre de usuario es obligatorio y único")]
        [RegularExpression(@"/^[a-zA-Z0-9]+$/", ErrorMessage = "No se aceptan simbolos")]
        public string UserName { get; set; }
        [DisplayName("Contraseña:")]
        [Required(ErrorMessage ="La contraseña es obligatorio")]
        public string Password { get; set; }
        [DisplayName("Fecha de nacimiento:")]
        [Required(ErrorMessage ="La fecha de nacimiento es obligatorio")]
        public string FechaNacimiento { get; set; }
        [DisplayName("Sexo:")]
        [Required(ErrorMessage ="Seleccione el sexo por favor")]
        public string Sexo { get; set; }
        [DisplayName("Celular personal:")]
        [Required(ErrorMessage ="El celular es obligatorio")]
        public string Celular { get; set; }

        public bool Estatus { get; set; }
        [DisplayName("Curp:")]
        [Required(ErrorMessage ="La curp del usuario es obligatorio")]
        public string Curp { get; set; }
        [DisplayName("Foto de perfil:")]
        public byte[] Imagen { get; set; }
        [DisplayName("Correo electrónico:")]
        [Required(ErrorMessage ="El correo electrónico es obligatorio")]
        public string Email { get; set; }
        [DisplayName("Rol del usuario:")]
        public Rol Rol { get; set; }
        public Direccion Direccion { get; set; }
        public List<object> Usuarios { get; set; }

    }
}
