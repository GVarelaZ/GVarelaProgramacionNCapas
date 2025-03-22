using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace PL_Web.Controllers
{
    public class CrudJSController : Controller
    {
        // GET: CrudJS
        public ActionResult GetAllJS()
        {
            return View();
        }

        [HttpGet] //GETALL
        public JsonResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            usuario.Nombre = "";
            usuario.ApellidoPaterno = "";
            usuario.ApellidoMaterno = "";
            usuario.Rol.IdRol = 0;
            ML.Result JsonGetAll = BL.Usuario.GetAllEFJS(usuario);
            JsonResult jsonResult = Json(JsonGetAll, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [HttpGet]  //ELIMINAR
        public JsonResult DeleteById(int idUsuario)
        {
            ML.Result result = BL.Usuario.DeleteEF(idUsuario);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //add
        //Update
        [HttpGet]//DDLRol
        public JsonResult DDLroles()
        {
            ML.Result rolResult = BL.Rol.GetAll();

            return Json(rolResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet] //ddlEstado
        public JsonResult GetEstados()
        {
            ML.Result EstadoResult = BL.Estado.EstadoGetAll();

            return Json(EstadoResult, JsonRequestBehavior.AllowGet);
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
        [HttpGet] // saber si es para actualizar o agregar
        public JsonResult form(int? idUsuario)
        {

            ML.Usuario usuario = new Usuario(); //Modelo vacia mas no nulo
            //ML.Estado estado = new ML.Estado();
            Result result = new Result();

            if (idUsuario == 0) // si viene vacio entonces es para agregar
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
                result = BL.Usuario.GetByIdEF(idUsuario.Value);
                usuario = (ML.Usuario)result.Object;
                usuario.imagenJS = usuario.Imagen == null ? "" : Convert.ToBase64String(usuario.Imagen);
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

            return Json(usuario, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult add(ML.Usuario usuario)
        {
            ML.Result result = new Result();

            if (ModelState.IsValid)
            {

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

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //regresar la informacion que me ha dado
                //llenar los ddl
                //mostrar los mensajes de error de DataAnnotations
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

                ML.Result resultRol = BL.Rol.GetAll(); // se guarda el resultado del get del rol
                usuario.Rol.Roles = resultRol.Objects;

                ML.Result resultEstado = BL.Estado.EstadoGetAll();
                usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstado.Objects;

                return Json(usuario, JsonRequestBehavior.AllowGet);
            }
        }
    }
}