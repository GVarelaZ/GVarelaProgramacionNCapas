using ML;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
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
                result = EnvioCorreo(candidato.cita, candidato.IdCandidato);

                result = BL.Cita.agregarCita(candidato);
                if (result.Correct)
                {
                    ViewBag.mensajeError = "Se ha agendado correctamente la cita del candidato seleccionado.";
                    return PartialView("_notificacion");
                }
                else
                {
                    ViewBag.mensajeError = "No se pudo agendar la cita, favor de verificar la informacion y volver a intentar.";
                    return PartialView("_notificacion");
                }
            }
            else
            {
                result = BL.Cita.actualizarCita(candidato);
                if (result.Correct)
                {
                    ViewBag.mensajeError = "Se ha actualizado correctamente la cita.";
                    return PartialView("_notificacion");

                }
                else
                {
                    ViewBag.mensajeError = "No se ha posido actualizar la cita, favor de verificar.";
                    return PartialView("_notificacion");
                }

            }

        }

        [NonAction]
        public Result EnvioCorreo(ML.Cita cita, int IdCandidato)
        {
            ML.Result envio = new ML.Result();
            envio = BL.Cita.ObtenerCandidato(IdCandidato);

            ML.Candidato candidato = (ML.Candidato)envio.Object;
            try
            {
                string correo = ConfigurationManager.AppSettings["correo"];
                string password = ConfigurationManager.AppSettings["password"];
                int puerto = int.Parse(ConfigurationManager.AppSettings["port"]);
                string smtpRuta = ConfigurationManager.AppSettings["smtpRuta"];

                string ruta = Server.MapPath("~/Content/Plantilla/Correo.html");
                string rutaImg = Server.MapPath("~/Content/Imagen/fondo.jpg");
                string rutaGetAll = string.Format("{0}://{1}{2}",
                                                Request.Url.Scheme,
                                                Request.Url.Authority,
                                        Url.Action("Inicio", "Candidato"));

                StreamReader plantilla = new StreamReader(ruta);
                string body = plantilla.ReadToEnd();

                body = body.Replace("{{usuario}}", $"{candidato.Nombre} {candidato.ApellidoPaterno}");
                if (candidato.cita.IdCita == 0)
                {
                    body = body.Replace("{{statusCita}}", "agendado");
                }
                body = body.Replace("{{FechaCita}}", $"{cita.FechaHora}");
                if (cita.Url == null)
                {
                    body = body.Replace("{{tipoCita}}", "presencial");
                    if (cita.IdCita == 1)
                    {
                        body = body.Replace("{{Medio}}", $"en el edificio reforma 199, piso 9");
                    }
                    else if (cita.IdCita == 2)
                    {
                        body = body.Replace("{{Medio}}", $"en el edificio reforma 199, piso 10");
                    }
                    else
                    {
                        body = body.Replace("{{Medio}}", $"en el edificio reforma 199, piso 14");
                    }
                }
                else
                {
                    body = body.Replace("{{tipoCita}}", "virtual");
                    body = body.Replace("{{Medio}}", $"por medio de Zoom en la liga {cita.Url}");

                }
                body = body.Replace("{{url}}", rutaGetAll);

                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

                LinkedResource inlineImage = new LinkedResource(rutaImg, MediaTypeNames.Image.Jpeg);
                inlineImage.ContentId = "{{myimage}}";
                inlineImage.TransferEncoding = TransferEncoding.Base64;
                avHtml.LinkedResources.Add(inlineImage);

                var smtpClient = new SmtpClient(smtpRuta)
                {
                    Port = puerto,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(correo, password),
                    EnableSsl = true
                };

                var mensaje = new MailMessage
                {
                    From = new MailAddress(correo, "Gustavo Varela"),
                    Subject = "envio de correo prueba",
                    Body = body,
                    IsBodyHtml = true
                };
                mensaje.AlternateViews.Add(avHtml);
                mensaje.To.Add("jguevaraflores3@gmail.com , gustavovarela256@gmail.com");
                smtpClient.Send(mensaje);

                envio.Correct = true;
            }
            catch (Exception ex)
            {
                envio.Correct = false;
                envio.ErrorMessage = ex.Message;
                envio.ex = ex;
            }

            return envio;
        }
    }
}