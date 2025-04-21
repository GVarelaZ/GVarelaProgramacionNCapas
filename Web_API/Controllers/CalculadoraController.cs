using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace Web_API.Controllers
{
    public class CalculadoraController : ApiController //controlador pero de la API
    {
        //[HttpPost]
        public IHttpActionResult Suma(double n1, double n2) //Metodo para entrar a la API
        {
            double suma = n1 + n2; //logico de la peticion

            return Content(HttpStatusCode.OK, suma); //retorna u ncontenido el cual devuelve el status de la peticion y un Json de respuesta
        }

        //[HttpPost]
        public IHttpActionResult Resta(double num1, double num2)
        {
            double resta = num1 - num2;

            return Content(HttpStatusCode.OK, resta);
        }

        //[HttpPost]
        public IHttpActionResult Multiplicacion(double nu1, double nu2)
        {
            double multiplicacion = nu1 * nu2;

            return Content(HttpStatusCode.OK, multiplicacion);
        }

        //[HttpPost]
        public IHttpActionResult Division(double numero1, double numero2)
        {
            double division = numero1 / numero2;

            return Content(HttpStatusCode.OK, division);
        }
    }
}
