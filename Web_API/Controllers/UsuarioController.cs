using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web_API.Controllers
{

    [RoutePrefix("api/usuario")] //Colocarle un endpoint por defecto a todos los metodos para no escribir todo
    public class UsuarioController : ApiController
    {
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            Usuario usuario = new Usuario();
            usuario.Rol = new Rol();

            usuario.Nombre = "";
            usuario.ApellidoPaterno = "";
            usuario.ApellidoMaterno = "";
            usuario.Rol.IdRol = 0;

            Result result = BL.Usuario.GetAllEF(usuario);
            if (result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, result);
            }
        }

        [HttpGet]
        [Route("GetById/{idUsuario}")]
        public IHttpActionResult GetById(int idUsuario)
        {
            Result result = BL.Usuario.GetByIdEF(idUsuario);
            if (result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, result);
            }
        }

        [HttpPost]
        [Route("add")] //edita el endPoint, para no usar el predeterminado
        public IHttpActionResult agregarUsuario([FromBody] ML.Usuario usuario) //mandamos un Json atraves del Body
        {
            Result result = BL.Usuario.AddEF(usuario);
            if (result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, result);
            }
        }

        [HttpDelete]
        [Route("Delete/{idUsuario}")]
        public IHttpActionResult delete(int idUsuario)
        {
            Result result = BL.Usuario.DeleteEF(idUsuario);
            if (result.Correct)
            {
                return Content(HttpStatusCode.OK, result);
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, result);
            }
        }

        [HttpPut]
        [Route("Update/{idUsuario}")]
        public IHttpActionResult update(int idUsuario,[FromBody] Usuario usuario)
        {
            usuario.idUsuario = idUsuario;
            Result result = BL.Usuario.ChangeEF(usuario);

            if (result.Correct)
            {
                return Content(HttpStatusCode.OK,result);
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, result);
            }
        }
    }
}
