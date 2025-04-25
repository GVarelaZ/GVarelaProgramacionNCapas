using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL_Web.Controllers
{
    public class AgendarCitaController : Controller
    {
        // GET: AgendarCita
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

        public ActionResult formCita(int? IdCandidato, int? IdCita) { 
        
            Result result = new Result();
            Candidato candidato = new Candidato();

            if (IdCandidato == 0 && IdCita == 0)
            {
                
            }
            else
            {

            }

            return View(candidato);
        }
    }
}