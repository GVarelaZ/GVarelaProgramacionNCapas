using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace PL_Web.Controllers
{
    public class EnvioCorreoController : Controller
    {
        // GET: EnvioCorreo
        public ActionResult Inicio()
        {
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

                body = body.Replace("{{usuario}}", "Jorge");
                body = body.Replace("{{statusCita}}", "agendado");
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
                    From = new MailAddress(correo,"Gustavo Varela"),
                    Subject = "envio de correo prueba",
                    Body = body,
                    IsBodyHtml = true
                };
                mensaje.AlternateViews.Add(avHtml);
                mensaje.To.Add("jguevaraflores3@gmail.com");
                smtpClient.Send(mensaje);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
    }
}