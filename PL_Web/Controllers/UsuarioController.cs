using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL_Web.Controllers
{
    public class UsuarioController : Controller // controlador que espera una accion 
    {
        // GET: Usuario
        [HttpGet]  //DECORADOR que es lo que espera el controlador para hacer la accion
        public ActionResult GetAll() // ActionMethod = metodo de accion para alguna salida (view, json, etc)
        {
            ML.Usuario usuario = new Usuario();
            usuario.Rol = new Rol();
            usuario.Nombre = "";
            usuario.ApellidoPaterno = "";
            usuario.ApellidoMaterno = "";
            usuario.Rol.IdRol = 0;
            ML.Result result = BL.Usuario.GetAllEF(usuario);

            if (result.Correct)
            {
                //Se obtiene toda la informacion que viene del BL por medio de result

                usuario.Usuarios = result.Objects; // > 0
                                                   // lo que trae el result se lo pasa a materia para que esta se envie
            }
            else
            {
                //Si no trae informacion
                usuario.Usuarios = new List<object> { }; // = 0
            }

            result = BL.Rol.GetAll();
            usuario.Rol.Roles = result.Objects;

            return View(usuario);
        }

        [HttpPost]
        public ActionResult GetAll(Usuario usuarioView)
        {
            usuarioView.ApellidoPaterno = usuarioView.ApellidoPaterno == null ? "" : usuarioView.ApellidoPaterno;
            usuarioView.Nombre = usuarioView.Nombre == null ? "" : usuarioView.Nombre; //Condicion termario si es null se hace vacio si no si queda el valor de la propiedad
            usuarioView.ApellidoMaterno = usuarioView.ApellidoMaterno == null ? "" : usuarioView.ApellidoMaterno;

            Result result = BL.Usuario.GetAllEF(usuarioView);

            Usuario usuario = new Usuario();
            usuario.Rol = new Rol();

            if (result.Correct)
            {
                //Se obtiene toda la informacion que viene del BL por medio de result

                usuario.Usuarios = result.Objects; // > 0
                                                   // lo que trae el result se lo pasa a materia para que esta se envie
            }
            else
            {
                //Si no trae informacion
                usuario.Usuarios = new List<object> { }; // = 0
            }

            result = BL.Rol.GetAll();
            usuario.Rol.Roles = result.Objects;

            return View(usuario);
        }
        [HttpGet] //Mostrar una vista

        public ActionResult Form(int? IdUsuario) //el ? es para decir que puede o no recibir un valor 
        {

            ML.Usuario usuario = new Usuario(); //Modelo vacia mas no nulo
            //ML.Estado estado = new ML.Estado();
            Result result = new Result();

            if (IdUsuario == null) // si viene vacio entonces es para agregar
            {
                //ACTUALIZAMOS PERO MOSTRAMOS LOS VALORES EN EL FORMULARIO
                usuario.Rol = new Rol(); //Se abre la puerta para el uso del rol
                usuario.Direccion = new Direccion();
                usuario.Direccion.Colonia = new Colonia();
                usuario.Direccion.Colonia.Municipio = new Municipio();
                usuario.Direccion.Colonia.Municipio.Estado = new Estado();
                //UNBOXING
                usuario.Direccion.Colonia.Colonias = new List<object>();
                usuario.Direccion.Colonia.Municipio.Municipios = new List<object>();
            }
            else
            {
                result = BL.Usuario.GetByIdEF(IdUsuario.Value);
                usuario = (ML.Usuario)result.Object;
                if (usuario.Direccion.Colonia.Municipio.Estado.IdEstado == 0)
                {
                    usuario.Direccion.Colonia.Colonias = new List<object>();
                    usuario.Direccion.Colonia.Municipio.Municipios = new List<object>();
                }
                else
                {
                    result = BL.Municipio.MunicipioGetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
                    usuario.Direccion.Colonia.Municipio.Municipios = result.Objects;
                    result = BL.Colonia.ColoniaGetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);
                    usuario.Direccion.Colonia.Colonias = result.Objects;
                }
            }

            ML.Result resultRol = BL.Rol.GetAll(); // se guarda el resultado del get del rol
            usuario.Rol.Roles = resultRol.Objects;

            ML.Result resultEstado = BL.Estado.EstadoGetAll();
            usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstado.Objects;

            return View(usuario);
        }

        [HttpPost]  //CACHAR INFORMACIOM DEL USUARIO, ya se ha de agregar o actualizar registros

        public ActionResult Form(ML.Usuario usuario)
        {
            ML.Result result = new Result();

            HttpPostedFileBase file = Request.Files["imagenCargada"];  // Obtenemos un archivo que viene del input del formulario 

            if (file != null && file.ContentLength != 0) //si hay informacion
            {
                //Convertir la imagen en un arreglo de Byte
                usuario.Imagen = ConvertirArrayBytes(file);  //llamamos el metodo para la conversion y se lo mandamos al modelo para que lo guarde
            }


            if (usuario.idUsuario == 0)  //Agregar Usuario
            {
                result = BL.Usuario.AddEF(usuario);
            }
            else
            {
                if (usuario.Direccion.IdDireccion == 0)
                {
                    result = BL.Usuario.UsuarioUpdateAddDireccion(usuario);
                }
                else
                {
                    result = BL.Usuario.ChangeEF(usuario); //Actualizar usuario

                }
            }

            return RedirectToAction("GetAll");
        }

        [HttpGet]  //Eliminar un registro seleccionado
        public ActionResult Delete(int IdUsuario)
        {
            Usuario usuario = new Usuario();
            Result result = BL.Usuario.DeleteEF(IdUsuario);

            if (result.Correct)
            {
                return RedirectToAction("GetAll");
            }

            return View();
        }

        [HttpPost]
        public JsonResult UpdateStatus(int Idsuario, bool Status) //Tiene  un tipo de dato Json
        {
            ML.Result JsonResult = BL.Usuario.GetStatus(Idsuario, Status);
            return Json(JsonResult, JsonRequestBehavior.AllowGet);//Devuelve un jason y permite cualquier peticion que se ha get
        }

        [HttpGet]
        public JsonResult MunicipioGetByIdEstado(int idEstado)
        {
            Result JsonResult = BL.Municipio.MunicipioGetByIdEstado(idEstado); //ejecucion del metodo 

            return Json(JsonResult, JsonRequestBehavior.AllowGet); //devuelve un json con la respuesta obtenida del metodo
        }

        [HttpGet]
        public JsonResult ColoniaGetByIdMunicipio(int idMunicipio)
        {
            Result JsonResult = BL.Colonia.ColoniaGetByIdMunicipio(idMunicipio);

            return Json(JsonResult, JsonRequestBehavior.AllowGet);
        }

        //Metodo para convertir una imagen a []bytes
        public byte[] ConvertirArrayBytes(HttpPostedFileBase foto)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(foto.InputStream);
            byte[] data = reader.ReadBytes((int)foto.ContentLength);

            return data;
        }
    }
}