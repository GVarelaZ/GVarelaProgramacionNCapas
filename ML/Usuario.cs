using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
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
        [RegularExpression(@"[a-zA-Z\s]+", ErrorMessage = "Solo se aceptan letras")]
        public string Nombre { get; set; }

        [DisplayName("Apellido paterno:")]
        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        [RegularExpression(@"[a-zA-Z\s]+", ErrorMessage = "Solo se aceptan letras")]
        public string ApellidoPaterno { get; set; }

        [DisplayName("Apellido materno:")]
        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        [RegularExpression(@"[a-zA-Z\s]", ErrorMessage = "Solo se aceptan letras")]
        public string ApellidoMaterno { get; set; }

        [DisplayName("Telefono de casa:")]
        [Required(ErrorMessage = "El telefono es obligatorio")]
        [RegularExpression(@"[0-9]{1,10}", ErrorMessage = "Solo ingresar números")]
        public string Telefono { get; set; }

        [DisplayName("Nombre de usuario:")]
        [Required(ErrorMessage = "El nombre de usuario es obligatorio y único")]
        [RegularExpression(@"[a-zA-Z][a-zA-Z0-9-_]{3,32}", ErrorMessage = "No se aceptan simbolos")]
        public string UserName { get; set; }

        [DisplayName("Contraseña:")]
        [Required(ErrorMessage = "La contraseña es obligatorio")]
        [RegularExpression(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$", ErrorMessage = "La contraseña debe de contener min. 8 caracteres, 1 mayuscula, 1 minuscula, numeros y un simbolo")]
        public string Password { get; set; }

        [DisplayName("Fecha de nacimiento:")]
        [Required(ErrorMessage = "La fecha de nacimiento es obligatorio")]
        public string FechaNacimiento { get; set; }

        [DisplayName("Sexo:")]
        [Required(ErrorMessage = "Seleccione el sexo por favor")]
        public string Sexo { get; set; }

        [DisplayName("Celular personal:")]
        [Required(ErrorMessage = "El celular es obligatorio")]
        [RegularExpression(@"[0-9]{1,10}$", ErrorMessage = "Solo ingresar números")]
        public string Celular { get; set; }

        public bool Estatus { get; set; }

        [DisplayName("Curp:")]
        [Required(ErrorMessage = "La curp del usuario es obligatorio")]
        [RegularExpression(@"[A-Z]{1}[AEIOU]{1}[A-Z]{2}[0-9]{2}(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[HM]{1}(AS|BC|BS|CC|CS|CH|CL|CM|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)[B-DF-HJ-NP-TV-Z]{3}[0-9A-Z]{1}[0-9]{1}", ErrorMessage = "No es un curp vàlido")]
        public string Curp { get; set; }

        [DisplayName("Foto de perfil:")]
        public byte[] Imagen { get; set; }

        [DisplayName("Correo electrónico:")]
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [RegularExpression(@"\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b", ErrorMessage = "No es un email válido")]
        public string Email { get; set; }
        public Rol Rol { get; set; }
        public Direccion Direccion { get; set; }
        public List<object> Usuarios { get; set; }

    }
}
