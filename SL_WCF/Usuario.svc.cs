using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SL_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Usuario" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Usuario.svc or Usuario.svc.cs at the Solution Explorer and start debugging.
    public class Usuario : IUsuario
    {
        public SL_WCF.Result AddWebService(ML.Usuario usuario) //define el metodo de la interfaz
        {
            ML.Result result = BL.Usuario.AddEF(usuario); //ingresa a BL para consumirlo
            Exception error = result.ex != null ? result.ex : new Exception();
            string inner = error.InnerException == null ? "" : error.InnerException.ToString();
            return new Result
            {
                Correct = result.Correct,
                ErrorMessage = result.ErrorMessage + inner,
                Object = result.Object,
                Objects = result.Objects
            };
        }

        public SL_WCF.Result UpdateWebService(ML.Usuario usuario)
        {
            //ML.Result usuarioGetId = BL.Usuario.GetByIdEF(usuario.idUsuario);
            //ML.Usuario usuarioResult = (ML.Usuario)usuarioGetId.Object;
            //usuarioResult.Nombre = usuario.Nombre == "" ? usuarioResult.Nombre : usuario.Nombre;
            //usuarioResult.ApellidoPaterno = usuario.ApellidoPaterno == "" ? usuarioResult.ApellidoPaterno : usuario.ApellidoPaterno;
            //usuarioResult.ApellidoMaterno = usuario.ApellidoMaterno == "" ? usuarioResult.ApellidoMaterno : usuario.ApellidoMaterno;
            //usuarioResult.Telefono = usuario.Telefono == "" ? usuarioResult.Telefono : usuario.Telefono;
            //usuarioResult.UserName = usuario.UserName == "" ? usuarioResult.UserName : usuario.UserName;
            //usuarioResult.Password = usuario.Password == "" ? usuarioResult.Password : usuario.Password;
            //usuarioResult.FechaNacimiento = usuario.FechaNacimiento == "" ? usuarioResult.FechaNacimiento : usuario.FechaNacimiento;
            //usuarioResult.Sexo = usuario.Sexo == "" ? usuarioResult.Sexo : usuario.Sexo;
            //usuarioResult.Celular = usuario.Celular == "" ? usuarioResult.Celular : usuario.Celular;
            //usuarioResult.Rol.IdRol = usuario.Rol.IdRol == 0 ? usuarioResult.Rol.IdRol : usuario.Rol.IdRol;
            //usuarioResult.Email = usuario.Email == "" ? usuarioResult.Email : usuario.Email;
            //usuarioResult.Direccion.Calle = usuario.Direccion.Calle == "" ? usuarioResult.Direccion.Calle : usuario.Direccion.Calle;
            //usuarioResult.Direccion.NumeroExterior = usuario.Direccion.NumeroExterior == "" ? usuarioResult.Direccion.NumeroExterior : usuario.Direccion.NumeroExterior;
            //usuarioResult.Direccion.NumeroInterior = usuario.Direccion.NumeroInterior == "" ? usuarioResult.Direccion.NumeroInterior : usuario.Direccion.NumeroInterior;
            //usuarioResult.Direccion.Colonia.IdColonia = usuario.Direccion.Colonia.IdColonia == 0 ? usuarioResult.Direccion.Colonia.IdColonia : usuario.Direccion.Colonia.IdColonia;

            ML.Result result = BL.Usuario.ChangeEF(usuario);
            Exception error = result.ex != null ? result.ex : new Exception();
            string inner = error.InnerException == null ? "" : error.InnerException.ToString();
            return new Result
            {
                Correct = result.Correct,
                ErrorMessage = result.ErrorMessage + inner,
                Object = result.Object,
                Objects = result.Objects
            };
        }

        public SL_WCF.Result DeleteWebService(int idUsuario)
        {
            ML.Result result = BL.Usuario.DeleteEF(idUsuario);
            Exception error = result.ex != null ? result.ex : new Exception();
            string inner = error.InnerException == null ? "" : error.InnerException.ToString();
            return new Result
            {
                Correct = result.Correct,
                ErrorMessage = result.ErrorMessage + inner,
                Object = result.Object,
                Objects = result.Objects
            };
        }

        public SL_WCF.Result GetAllWebService(ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.GetAllEF(usuario);
            Exception error = result.ex != null ? result.ex : new Exception();
            string inner = error.InnerException == null ? "" : error.InnerException.ToString();
            return new Result
            {
                Correct = result.Correct,
                ErrorMessage = result.ErrorMessage + inner,
                Object = result.Object,
                Objects = result.Objects
            };
        }

        public SL_WCF.Result GetByIdWebService(int idUsuario)
        {
            ML.Result result = BL.Usuario.GetByIdEF(idUsuario);
            Exception error = result.ex != null ? result.ex : new Exception();
            string inner = error.InnerException == null ? "" : error.InnerException.ToString();
            return new Result
            {
                Correct = result.Correct,
                ErrorMessage = result.ErrorMessage + inner,
                Object = result.Object,
                Objects = result.Objects
            };
        }
    }
}
