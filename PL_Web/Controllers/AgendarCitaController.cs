using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PL_Web.Controllers
{
    public class AgendarCitaController : Controller
    {
        // GET: AgendarCita
        [HttpGet]
        public ActionResult Principal()
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
        public ActionResult Principal(ML.Vacante vacante)
        {
            ML.Result result = new ML.Result();
            ML.Candidato candidato = new ML.Candidato();
            candidato.vacante = new ML.Vacante();

            result = BL.Cita.obtenerCandidatosCita(vacante.IdVacante);

            if (result.Correct)
            {
                candidato.Candidatos = result.Objects;
            }

            result = BL.Vacante.VacantesGetAll();
            candidato.vacante.Vacantes = result.Objects;

            return View(candidato);
        }

        [HttpGet]
        public ActionResult formCita(int IdCandidato)
        {
            Result result = new Result();
            Candidato candidato = new Candidato();
            candidato.cita = new Cita();
            candidato.cita.estatusCita = new EstatusCita();
            candidato.cita.piso = new Piso();

            result = BL.Cita.ObtenerCandidato(IdCandidato);
            candidato = (Candidato)result.Object;

            result = BL.EstatusCita.obtenerEstatusCita();
            candidato.cita.estatusCita.estatusCitas = result.Objects;
            result = BL.Piso.obtenerPisos();
            candidato.cita.piso.Pisos = result.Objects;

            return View(candidato);

        }

        [HttpPost]
        public ActionResult FormCita(Candidato candidato)
        {
            ML.Result result = new ML.Result();

            if (candidato.cita.IdCita == 0)
            {
                //agregar
                result = BL.Cita.agregarCita(candidato);
                return RedirectToAction("Principal");

            }
            else
            {
                //editar
                result = BL.Cita.actualizarCita(candidato);
                return RedirectToAction("Principal");
            }
        }
    }
}