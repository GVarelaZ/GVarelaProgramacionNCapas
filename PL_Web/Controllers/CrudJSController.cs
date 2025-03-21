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

        [HttpGet]
        public JsonResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            usuario.Nombre = "";
            usuario.ApellidoPaterno = "";
            usuario.ApellidoMaterno = "";
            usuario.Rol.IdRol = 0;
            ML.Result JsonGetAll = BL.Usuario.GetAllEFJS(usuario);

            return Json(JsonGetAll, JsonRequestBehavior.AllowGet);
        }
    }
}