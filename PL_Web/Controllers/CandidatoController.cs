using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL_Web.Controllers
{
    public class CandidatoController : Controller
    {
        // GET: Candidato
        [HttpGet]
        public ActionResult Inicio()
        {
            ML.Result result = new ML.Result();
            ML.Candidato candidato = new ML.Candidato();
            candidato.vacante = new ML.Vacante();

            result = BL.Vacante.VacantesGetAll();

            if (result.Correct)
            {
                candidato.vacante.Vacantes = result.Objects;
            }

            return View(candidato);
        }

        [HttpPost]
        public ActionResult Inicio(ML.Vacante vacante)
        {
            ML.Result result = new ML.Result();
            ML.Candidato candidato = new ML.Candidato();
            candidato.vacante = new ML.Vacante();

            result = BL.Candidato.candidatoGetAll(vacante.IdVacante);

            if (result.Correct)
            {
                candidato.Candidatos = result.Objects;
            }

            result = BL.Vacante.VacantesGetAll();
            candidato.vacante.Vacantes = result.Objects;

            return View(candidato);
        }

        [HttpGet]
        public ActionResult EliminarCandidato(int IdCandidato)
        {
            ML.Result result = new ML.Result();

            result = BL.Candidato.deleteCandidatos(IdCandidato);

            if (result.Correct)

            {
                ViewBag.mensajeError = "El candidato seleccionado se ha eliminado correctamente.";
                return PartialView("_notificacion");
            }
            else
            {
                ViewBag.mensajeError = "El registro no se ha podido eliminar";
                return PartialView("_notificacion");
            }
        }

        [HttpGet]
        public ActionResult Formulario(int? IdCandidato)
        
        {
            ML.Result result = new ML.Result();
            ML.Candidato candidato = new ML.Candidato();

            candidato.bolsaTrabajo = new ML.BolsaTrabajo();
            candidato.carrera = new ML.Carrera();
            candidato.universidad = new ML.Universidad();
            candidato.vacante = new ML.Vacante();
            candidato.promedio = new ML.Promedio();

            if (IdCandidato == null)
            {

                candidato.universidad.Universidades = new List<object>();
                candidato.carrera.Carreras = new List<object>();
                candidato.bolsaTrabajo.BolsasTrabajo = new List<object>();
                candidato.vacante.Vacantes = new List<object>();
            }
            else
            {
                result = BL.Candidato.getByIdCandidato(IdCandidato.Value);
                candidato = (ML.Candidato)result.Object;
            }

            result = BL.Vacante.VacantesGetAll();
            candidato.vacante.Vacantes = result.Objects;
            result = BL.BolsaTrabajo.GetAllBolsaTrabajo();
            candidato.bolsaTrabajo.BolsasTrabajo = result.Objects;
            result = BL.Carrera.GetAllCarreras();
            candidato.carrera.Carreras = result.Objects;
            result = BL.Universidad.GetallUniversidades();
            candidato.universidad.Universidades = result.Objects;

            return View(candidato);
        }

        [HttpPost]  //modelIsValid
        public ActionResult Formulario(ML.Candidato candidato)
        {
            ML.Result result = new ML.Result();

            if(candidato.IdCandidato == 0)
            {
                result = BL.Candidato.agregarCandidato(candidato);
                if (result.Correct)
                {
                    ViewBag.mensajeError = "Se ha registrado correctamente al usuario ingresado.";
                    return PartialView("_notificacion");
                }
                else
                {
                    ViewBag.mensajeError = "No se pudo ingresar el registro, favor de verificar la informacion y volver a intentar.";
                    return PartialView("_notificacion");
                }
            }
            else
            {
                result = BL.Candidato.actualizarCandidato(candidato);

                if (result.Correct)
                {
                    ViewBag.mensajeError = "Se ha actualizado correctamente al usuario seleccionado.";
                    return PartialView("_notificacion");

                }
                else
                {
                    ViewBag.mensajeError = "No se ha podido actualizado correctamente al usuario seleccionado.";
                    return PartialView("_notificacion");
                }
            }
        }
    }
}