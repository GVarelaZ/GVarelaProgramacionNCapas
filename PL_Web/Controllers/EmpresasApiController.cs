using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL_Web.Controllers
{
    public class EmpresasApiController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            ML.Empresa empresa = new ML.Empresa();
            return View(empresa);
        }

        [HttpGet]
        public ActionResult Form(int? IdEmpresa)
        {
            ML.Result result = new ML.Result();
            ML.Empresa empresa = new ML.Empresa();

            if (IdEmpresa != null)
            {
                result = BL.Empresa.ObtenerEmpresa(IdEmpresa.Value);
                empresa = (ML.Empresa)result.Object;
            }

            return View(empresa);
        }

        [HttpPost]
        public ActionResult Form(ML.Empresa empresa)
        {
            ML.Result result = new ML.Result();

            if (empresa.IdEmpresa == 0)
            {
                result = BL.Empresa.AgregarEmpresa(empresa);

                if (result.Correct)
                {
                    ViewBag.Correct = true;
                    ViewBag.mensajeError = "Se ha registrado correctamente la empresa.";
                    return PartialView("_Avisos");
                }
                else
                {
                    ViewBag.Correct = false;
                    ViewBag.mensajeError = "No se pudo ingresar el registro, favor de verificar la informacion y volver a intentar.";
                    return PartialView("_Avisos");
                }
            }
            else
            {
                result = BL.Empresa.ActualizarEmpresa(empresa);

                if (result.Correct)
                {
                    ViewBag.Correct = true;
                    ViewBag.mensajeError = "Se ha actualizado correctamente la empresa.";
                    return PartialView("_Avisos");
                }
                else
                {
                    ViewBag.Correct = false;
                    ViewBag.mensajeError = "No se pudo actualizar el registro, favor de verificar la informacion y volver a intentar.";
                    return PartialView("_Avisos");
                }
            }


        }

        [HttpGet]
        public ActionResult Delete(int IdEmpresa)
        {
            ML.Result result = new ML.Result();

            result = BL.Empresa.EliminarEmpresa(IdEmpresa);

            if (result.Correct)
            {
                ViewBag.Correct = true;
                ViewBag.mensajeError = "Se ha eliminado correctamente la empresa";
                return PartialView("_Avisos");
            }
            else
            {
                ViewBag.Correct = false;
                ViewBag.mensajeError = "No se pudo eliminar el registro.";
                return PartialView("_Avisos");
            }

        }

        [HttpGet]
        public JsonResult GetAllEmpresas()
        {
            ML.Result result = new ML.Result();
            result = BL.Empresa.ObtenerEmpresas();

            if (result.Correct)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}