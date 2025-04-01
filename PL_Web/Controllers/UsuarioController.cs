using ML;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace PL_Web.Controllers
{
    public class UsuarioController : Controller // controlador que espera una accion 
    {
        // GET: Usuario
        [HttpGet]  //DECORADOR que es lo que espera el controlador para hacer la accion

        //Metodo consumiendo al web service con service reference
        /*public ActionResult GetAll() // ActionMethod = metodo de accion para alguna salida (view, json, etc)
        {
            ML.Result resultBL = new Result();

            ML.Usuario usuario = new Usuario();
            usuario.Rol = new Rol();
            usuario.Nombre = "";
            usuario.ApellidoPaterno = "";
            usuario.ApellidoMaterno = "";
            usuario.Rol.IdRol = 0;

            //GETALL CONSUMIDO DESDE EL CONTROLADOR HACIA EL BL
            //ML.Result result = BL.Usuario.GetAllEF(usuario);

            //GETALL OCNSUMIDO POR EL WEBSERVICES HACIA EL BL POR MEDIO DE SOAP
            UsuarioReference.UsuarioClient ObjetoWCF = new UsuarioReference.UsuarioClient(); // Se consume el web service 
            var result = ObjetoWCF.GetAllWebService(usuario); //se consume el metodo que viene del servicio
            if (result.Correct)
            {
                //Se obtiene toda la informacion que viene del BL por medio de result

                usuario.Usuarios = result.Objects.ToList(); // > 0
                                                            // lo que trae el result se lo pasa a materia para que esta se envie
            }
            else
            {
                //Si no trae informacion
                usuario.Usuarios = new List<object> { }; // = 0
            }

            resultBL = BL.Rol.GetAll();
            usuario.Rol.Roles = resultBL.Objects;
            return View(usuario);
        }*/

        // Metodo para consummir el web service desde el envio, lectura y respuesta de la peticion SOAP
        public ActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new Rol();

            string action = "http://tempuri.org/IUsuario/GetAllWebService"; // URL de la accion o metodo para consumir el web service 
            string url = "http://localhost:48690/Usuario.svc"; //URL del servicio a consumir
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("SOAPAction", action);
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Accept = "text/xml";
            request.Method = "POST";   //etiquetas parecidos a las de un ajax

            string soapEnviar = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"" xmlns:ml=""http://schemas.datacontract.org/2004/07/ML"" xmlns:arr=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
                               <soapenv:Header/>
                               <soapenv:Body>
                                  <tem:GetAllWebService>
                                     <tem:usuario>
                                        <ml:ApellidoMaterno></ml:ApellidoMaterno>
                                        <ml:ApellidoPaterno></ml:ApellidoPaterno>
                                        <ml:Nombre></ml:Nombre>
                                        <ml:Rol>
                                           <ml:IdRol>0</ml:IdRol>
                                        </ml:Rol>
                                     </tem:usuario>
                                  </tem:GetAllWebService>
                               </soapenv:Body>
                            </soapenv:Envelope>";  //XML a enviar al webservices de ti po SOAP

            using (Stream stream = request.GetRequestStream()) //Se enviara la solicitud
            {
                byte[] content = Encoding.UTF8.GetBytes(soapEnviar); // Se transforma el XML de tipo string a UTF8 en array bite
                stream.Write(content, 0, content.Length); // Se envia la peticion
            }
            //Se obtiene la respuesta
            try
            {
                using (WebResponse respuesta = request.GetResponse()) // se obtiene la respuesta del XML enviado
                {
                    using (StreamReader readerXML = new StreamReader(respuesta.GetResponseStream())) // Se lee el archivo XML de respuesta
                    {
                        string result = readerXML.ReadToEnd(); // se guarda el XML de respuesta

                        //Deserealizar el XML
                        ML.Result resultXML = deserealizarUsuarios(result); //se manda a un metodo diferente todo el result

                        if (resultXML.Correct)
                        {
                            //Se obtiene toda la informacion que viene del BL por medio de result

                            usuario.Usuarios = resultXML.Objects.ToList(); // > 0
                                                                           // lo que trae el result se lo pasa a materia para que esta se envie
                        }
                        else
                        {
                            //Si no trae informacion
                            usuario.Usuarios = new List<object> { }; // = 0
                        }
                        ML.Result resultBL = new ML.Result();

                        resultBL = BL.Rol.GetAll();
                        usuario.Rol.Roles = resultBL.Objects;
                        return View(usuario);
                    }
                }
            }
            catch (Exception EX)
            {
                ViewBag.mensajeError = "Los registros no se han podido obtener correctamente";
                return PartialView("_Avisos");

            }

        }

        [HttpPost] //Busqueda abierta
        public ActionResult GetAll(Usuario usuarioView)
        {
            usuarioView.ApellidoPaterno = usuarioView.ApellidoPaterno == null ? "" : usuarioView.ApellidoPaterno;
            usuarioView.Nombre = usuarioView.Nombre == null ? "" : usuarioView.Nombre; //Condicion termario si es null se hace vacio si no si queda el valor de la propiedad
            usuarioView.ApellidoMaterno = usuarioView.ApellidoMaterno == null ? "" : usuarioView.ApellidoMaterno;

            //ESTE RESULT CONSUME DIRECTAMENTE AL BL
            //Result result = BL.Usuario.GetAllEF(usuarioView);

            //ESTE OBJETO CONSUME EL WEB SERVICES DE TIPO SOAP
            UsuarioReference.UsuarioClient ObjetoWCF = new UsuarioReference.UsuarioClient();
            var result = ObjetoWCF.GetAllWebService(usuarioView);

            Result resultBL = new Result();
            Usuario usuario = new Usuario();
            usuario.Rol = new Rol();

            if (result.Correct)
            {
                //Se obtiene toda la informacion que viene del BL por medio de result

                usuario.Usuarios = result.Objects.ToList(); // > 0
                                                            // lo que trae el result se lo pasa a materia para que esta se envie
            }
            else
            {
                //Si no trae informacion
                usuario.Usuarios = new List<object> { }; // = 0
            }

            resultBL = BL.Rol.GetAll();
            usuario.Rol.Roles = resultBL.Objects;

            return View(usuario);
        }

        [HttpGet] //Mostrar una vista
        public ActionResult Form(int? IdUsuario) //el ? es para decir que puede o no recibir un valor 
        {

            ML.Usuario usuario = new Usuario(); //Modelo vacia mas no nulo
            //ML.Estado estado = new ML.Estado();
            Result result = new Result();

            if (IdUsuario == null) // si viene vacio entonces es para agregar
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
                //RESULT QUE CONSUME EL BL DIRECTAMENTE
                //result = BL.Usuario.GetByIdEF(IdUsuario.Value);

                //OBJETO QUE CONSUME PRIMERO EL WEB SERVICES QUE CONSUME AL BL
                //UsuarioReference.UsuarioClient ObjetoWSF = new UsuarioReference.UsuarioClient();
                //var resultWSF = ObjetoWSF.GetByIdWebService(IdUsuario.Value);

                //METODO QUE CONSUME EL WEB SERVICE CON EL XML DESDE CERO
                ML.Result resultWSF = GetByIdWebService(IdUsuario.Value);

                usuario = (ML.Usuario)resultWSF.Object;
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

            return View(usuario);
        }

        [HttpPost]  //CACHAR INFORMACIOM DEL USUARIO, ya se ha de agregar o actualizar registros
        public ActionResult Form(ML.Usuario usuario)
        {
            ML.Result resultCtrl = new Result();

            if (ModelState.IsValid)
            {

                HttpPostedFileBase file = Request.Files["imagenCargada"];  // Obtenemos un archivo que viene del input del formulario 

                if (file != null && file.ContentLength != 0) //si hay informacion
                {
                    //Convertir la imagen en un arreglo de Byte
                    usuario.Imagen = ConvertirArrayBytes(file);  //llamamos el metodo para la conversion y se lo mandamos al modelo para que lo guarde
                }


                if (usuario.idUsuario == 0)  //Agregar Usuario
                {
                    //  El controlador consume directamente al BL
                    //result = BL.Usuario.AddEF(usuario);

                    //El controlador consume al web services para despues consumir al BL
                    //UsuarioReference.UsuarioClient resultSWF = new UsuarioReference.UsuarioClient();
                    //var result = resultSWF.AddWebService(usuario);

                    //FUNCION QUE REALIZA LA PETICION DESDE EL WEBSERVICE ENVIANDO EL XML COMPLETO
                    var result = InsertarActualizar(usuario);

                    if (result.Correct)
                    {
                        ViewBag.mensajeError = "Se ha registrado correctamente al usuario ingresado.";
                        return PartialView("_Avisos");
                    }
                    else
                    {
                        ViewBag.mensajeError = "No se pudo ingresar el registro, favor de verificar la informacion y volver a intentar.";
                        return PartialView("_Avisos");
                    }
                }
                else
                {
                    if (usuario.Direccion.IdDireccion == 0)
                    {
                        resultCtrl = BL.Usuario.UsuarioUpdateAddDireccion(usuario);
                        ViewBag.mensajeError = "Se ha actualizado correctamente al usuario seleccionado.";
                        return PartialView("_Avisos");
                    }
                    else
                    {
                        ////resultCtrl = BL.Usuario.ChangeEF(usuario); //Actualizar usuario

                        ///CONSUMIR EL WEB SERVICE DESDE LA REFERENCIA CON EL WEB SERVICE
                        //UsuarioReference.UsuarioClient usuarioWCF = new UsuarioReference.UsuarioClient();
                        //var result = usuarioWCF.UpdateWebService(usuario);

                        //CONSUMIR EL WEB SERVICE ENVIANDO TODO EL XML DE LA PETICION DESDE CERO
                        var result = InsertarActualizar(usuario);

                        if (result.Correct)
                        {
                            ViewBag.mensajeError = "Se ha actualizado correctamente al usuario seleccionado.";
                            return PartialView("_Avisos");

                        }
                        else
                        {
                            ViewBag.mensajeError = "No se ha podido actualizado correctamente al usuario seleccionado.";
                            return PartialView("_Avisos");
                        }
                    }
                }

                //return RedirectToAction("GetAll");
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
                    resultCtrl = BL.Municipio.MunicipioGetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
                    usuario.Direccion.Colonia.Municipio.Municipios = resultCtrl.Objects;
                    resultCtrl = BL.Colonia.ColoniaGetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);
                    usuario.Direccion.Colonia.Colonias = resultCtrl.Objects;
                }

                ML.Result resultRol = BL.Rol.GetAll(); // se guarda el resultado del get del rol
                usuario.Rol.Roles = resultRol.Objects;

                ML.Result resultEstado = BL.Estado.EstadoGetAll();
                usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstado.Objects;

                return View(usuario);
            }

        }

        [HttpGet]  //Eliminar un registro seleccionado
        /*public ActionResult Delete(int IdUsuario)
        {
            //Result result = BL.Usuario.DeleteEF(IdUsuario);

            UsuarioReference.UsuarioClient ObjetoWCF = new UsuarioReference.UsuarioClient();
            var result = ObjetoWCF.DeleteWebService(IdUsuario);

            if (result.Correct)
            {
                ViewBag.mensajeError = "El usuario seleccionado se ha eliminado correctamente.";
                return PartialView("_Avisos");
            }
            else
            {
                ViewBag.mensajeError = "El usuario seleccionado no se ha eliminado correctamente.";
                return PartialView("_Avisos");
            }

        }*/

        // Metodo para consummir el web service desde el envio, lectura y respuesta de la peticion SOAP
        public ActionResult Delete(int IdUsuario)
        {
            ML.Result result = new Result();

            string action = "http://tempuri.org/IUsuario/DeleteWebService";
            string url = "http://localhost:48690/Usuario.svc";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("SOAPAction", action);
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Method = "POST";
            request.Accept = "text/xml";  //Configurar nuestro xml para poder enviarse parecido a un JSON

            string soapEnviar = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                                       <soapenv:Header/>
                                       <soapenv:Body>
                                          <tem:DeleteWebService>
                                             <tem:idUsuario>{IdUsuario}</tem:idUsuario>
                                          </tem:DeleteWebService>
                                       </soapenv:Body>
                                    </soapenv:Envelope>";  //XML de nuestra peticion al web services

            using (Stream stream = request.GetRequestStream())
            {
                byte[] content = Encoding.UTF8.GetBytes(soapEnviar);
                stream.Write(content, 0, content.Length);
            } // Envio de peticion por medio del XML

            try //Obtener la respuesta a la peticion
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string xml = reader.ReadToEnd(); // Se lee la respuesta del Web service
                        var xdoc = XDocument.Parse(xml);
                        var xresult = xdoc.Descendants().FirstOrDefault(e =>
                                        e.Name.LocalName == "Correct" &&
                                        e.GetDefaultNamespace().NamespaceName == "http://tempuri.org/"); //obtengo la etiqueta completa por su nombre

                        result.Correct = bool.Parse(xresult.Value);


                        if (result.Correct)
                        {
                            ViewBag.mensajeError = "El usuario seleccionado se ha eliminado correctamente.";
                            return PartialView("_Avisos");
                        }
                        else
                        {
                            ViewBag.mensajeError = "El registro no se ha podido eliminar";
                            return PartialView("_Avisos");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.mensajeError = "El registro no se ha podido eliminar";
                return PartialView("_Avisos");
            }
        }

        [HttpPost]
        public JsonResult UpdateStatus(int Idsuario, bool Status) //Tiene  un tipo de dato Json
        {
            ML.Result JsonResult = BL.Usuario.GetStatus(Idsuario, Status);
            return Json(JsonResult, JsonRequestBehavior.AllowGet);//Devuelve un jason y permite cualquier peticion que se ha get
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

        //Metodo para convertir una imagen a []bytes
        public byte[] ConvertirArrayBytes(HttpPostedFileBase foto)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(foto.InputStream);
            byte[] data = reader.ReadBytes((int)foto.ContentLength);

            return data;
        }

        [HttpPost]

        public ActionResult CargaMasiva()
        {
            if (Session["RutaExcel"] == null)
            {

                HttpPostedFileBase archivo = Request.Files["archivoCargado"]; //recibe la peticion del input del formulario

                string extensionExcel = ConfigurationManager.AppSettings["ExtensionExcel"].ToString(); //variable que se encuentra en el appSettings del webConfig
                string extensionAceptada = extensionExcel; //formato de excel, solo permitido

                if (archivo.ContentLength > 0) //si el usuario si ingreso un archivo correcto
                {
                    string extensionArchivo = Path.GetExtension(archivo.FileName);  //path.getExtension obtiene la extension del archivo cargado

                    if (extensionAceptada == extensionArchivo)  // evalua si coinciden las extensiones del archivo
                    {
                        //se hace una ruta para guardar una copia del archivo que el usuario ingreso para manipularlo
                        string ruta = Server.MapPath("~/CargaMasiva/") + Path.GetFileNameWithoutExtension(archivo.FileName) + "-" +  //~ crea una ruta relativa con restricciones para mayor seguridad
                            DateTime.Now.ToString("ddMMyyyyHmmssff") + extensionAceptada; //Se crea la ruta donde se va a guardar el archivo con el nombre y extension

                        if (!System.IO.File.Exists(ruta))
                        { //si la ruta y archivo no existen entonces se va a guardar archivo

                            archivo.SaveAs(ruta); //Se guarda el archivo en la ruta designada

                            //se usara el proveedor de OLEDB para leer y manipular el excel
                            string cadenaConexion = ConfigurationManager.ConnectionStrings["OleDbConnection"] + ruta; //Cadena de conexion para usar OLEDB

                            ML.Result resultExcel = BL.Excel.LeerExcel(cadenaConexion);

                            if (resultExcel.Correct)
                            {
                                //El excel se a leido correctamente                           
                                //hacer validaciones
                                ML.Result validacionesResult = BL.Excel.ValidarExcel(resultExcel.Objects);

                                if (validacionesResult.Objects.Count > 0)
                                {
                                    //Hubo un error
                                    //mostrar una vista o una tabla
                                    ViewBag.opcion = 1;
                                    ViewBag.MensajesError = validacionesResult.Objects;
                                    return PartialView("_Modal");
                                }
                                else
                                {
                                    Session["RutaExcel"] = ruta;
                                    Session["nombreArchivo"] = Path.GetFileName(archivo.FileName);
                                    ViewBag.MensajeExito = "El archivo a sido validado y actualmente no se han encontrado errores, ya se pueden ingresar correctamente.";
                                    return PartialView("_Modal");
                                }
                            }
                            else
                            {
                                //No se pudo acceder al excel
                                //mostrar vista parcial de error
                                ViewBag.MensajesError = "No se ha cargado ningun archivo, favor de verificar y volver a intentar";
                                return PartialView("_Modal");

                            }
                        }
                        else
                        {
                            //Vista parcial
                            //El archivo ya existe
                            ViewBag.MensajeError = "El archivo ya existe favor de verificar";
                            return PartialView("_Modal");
                        }
                    }
                    else
                    {
                        //vista parcial
                        //el archivo no es un excel
                        ViewBag.MensajeError = "El archivo ingresado no es un excel, verificar";
                        return PartialView("_Modal");
                    }
                }
                else
                {
                    //vista parcial
                    //No existe ningun archivo cargado
                    ViewBag.MensajeError = "No existe ningun archivo cargado actualmente, ingresar alguno";
                    return PartialView("_Modal");
                }
            }
            else
            {
                string cadenaConexion = ConfigurationManager.ConnectionStrings["OleDbConnection"] + Session["RutaExcel"].ToString();

                ML.Result leerResult = BL.Excel.LeerExcel(cadenaConexion);

                if (leerResult.Objects.Count > 0)
                {
                    int contadorErrores = 0;
                    foreach (ML.Usuario usuario in leerResult.Objects)
                    {
                        ML.Result insertResult = BL.Usuario.AddEF(usuario);
                        if (!insertResult.Correct)
                        {
                            //mostrar error salio
                            contadorErrores++;
                        }
                        ViewBag.RegistrosNoInsertados += " , " + contadorErrores;

                    }
                    //cuantos insertes son correctos
                    //cuantos insertes son incorrectos
                    //cuales estuvieron mal
                    ViewBag.mensajeInsertar += " | Total de registros obtenidos para su inserccion: " + (leerResult.Objects.Count) + " |";
                    ViewBag.mensajeInsertar += "Registros insertados correctamente: " + (leerResult.Objects.Count - contadorErrores) + " , favor de verificarlos |";
                    ViewBag.mensajeInsertar += "Registros que no han podido insertarse: " + (contadorErrores) + " , favor de revisarlos, he intentar de nuevo";
                    Session["RutaExcel"] = null;

                    return PartialView("_Modal");
                }
                else
                {
                    //error
                }
            }

            Session["RutaExcel"] = null;

            return RedirectToAction("GetAll");
        }

        [NonAction]
        private ML.Result deserealizarUsuarios(string XML)
        {
            ML.Result result = new Result();
            result.Objects = new List<object>();

            var usuarioDocument = XDocument.Parse(XML); // Convertir el string a un Xdocument

            //Acceder a los registro obtenidos del XML de respuesta
            var objetos = usuarioDocument.Descendants("{http://schemas.microsoft.com/2003/10/Serialization/Arrays}anyType");

            if (objetos.Count() > 0)
            {

                foreach (var usuarioXML in objetos)
                {
                    ML.Usuario usuario = new ML.Usuario();
                    usuario.Rol = new Rol();
                    usuario.Direccion = new Direccion();
                    usuario.Direccion.Colonia = new Colonia();
                    usuario.Direccion.Colonia.Municipio = new Municipio();
                    usuario.Direccion.Colonia.Municipio.Estado = new Estado();

                    int idUsuario;
                    bool status;
                    // Si el idUsuario es nulo o vacio entonces no hace la conversion, sino entonces hace la conversion y lo retorna en una variable
                    int.TryParse(usuarioXML.Element("{http://schemas.datacontract.org/2004/07/ML}idUsuario")?.Value, out idUsuario);
                    usuario.idUsuario = idUsuario;
                    bool.TryParse(usuarioXML.Element("{http://schemas.datacontract.org/2004/07/ML}Estatus")?.Value, out status);

                    usuario.Nombre = usuarioXML.Element("{http://schemas.datacontract.org/2004/07/ML}Nombre")?.Value ?? string.Empty;
                    usuario.ApellidoPaterno = usuarioXML.Element("{http://schemas.datacontract.org/2004/07/ML}ApellidoPaterno")?.Value ?? string.Empty;
                    usuario.ApellidoMaterno = usuarioXML.Element("{http://schemas.datacontract.org/2004/07/ML}ApellidoMaterno")?.Value ?? string.Empty;
                    usuario.Telefono = usuarioXML.Element("{http://schemas.datacontract.org/2004/07/ML}Telefono")?.Value ?? string.Empty;
                    usuario.UserName = usuarioXML.Element("{http://schemas.datacontract.org/2004/07/ML}UserName")?.Value ?? string.Empty;
                    usuario.Password = usuarioXML.Element("{http://schemas.datacontract.org/2004/07/ML}Password")?.Value ?? string.Empty;
                    usuario.FechaNacimiento = usuarioXML.Element("{http://schemas.datacontract.org/2004/07/ML}FechaNacimiento")?.Value ?? string.Empty;
                    usuario.Sexo = usuarioXML.Element("{http://schemas.datacontract.org/2004/07/ML}Sexo")?.Value ?? string.Empty;
                    usuario.Celular = usuarioXML.Element("{http://schemas.datacontract.org/2004/07/ML}Celular")?.Value ?? string.Empty;
                    usuario.Estatus = status;
                    usuario.Curp = usuarioXML.Element("{http://schemas.datacontract.org/2004/07/ML}Curp")?.Value ?? string.Empty;
                    usuario.Email = usuarioXML.Element("{http://schemas.datacontract.org/2004/07/ML}Email")?.Value ?? string.Empty;

                    var Rol = usuarioXML.Element("{http://schemas.datacontract.org/2004/07/ML}Rol");
                    usuario.Rol.Nombre = Rol.Element("{http://schemas.datacontract.org/2004/07/ML}Nombre")?.Value ?? string.Empty;

                    var Direccion = usuarioXML.Element("{http://schemas.datacontract.org/2004/07/ML}Direccion");
                    int idDireccion;
                    int.TryParse(Direccion.Element("{http://schemas.datacontract.org/2004/07/ML}IdDireccion")?.Value, out idDireccion);
                    usuario.Direccion.IdDireccion = idDireccion;
                    usuario.Direccion.Calle = Direccion.Element("{http://schemas.datacontract.org/2004/07/ML}Calle")?.Value ?? string.Empty;
                    usuario.Direccion.NumeroInterior = Direccion.Element("{http://schemas.datacontract.org/2004/07/ML}NumeroInterior")?.Value ?? string.Empty;
                    usuario.Direccion.NumeroExterior = Direccion.Element("{http://schemas.datacontract.org/2004/07/ML}NumeroExterior")?.Value ?? string.Empty;

                    var Colonia = Direccion.Element("{http://schemas.datacontract.org/2004/07/ML}Colonia");
                    usuario.Direccion.Colonia.Nombre = Colonia.Element("{http://schemas.datacontract.org/2004/07/ML}Nombre")?.Value ?? string.Empty;
                    usuario.Direccion.Colonia.CodigoPostal = Colonia.Element("{http://schemas.datacontract.org/2004/07/ML}CodigoPostal")?.Value ?? string.Empty;

                    var Municipio = Colonia.Element("{http://schemas.datacontract.org/2004/07/ML}Municipio");
                    usuario.Direccion.Colonia.Municipio.Nombre = Municipio.Element("{http://schemas.datacontract.org/2004/07/ML}Nombre")?.Value ?? string.Empty;

                    var Estado = Municipio.Element("{http://schemas.datacontract.org/2004/07/ML}Estado");
                    usuario.Direccion.Colonia.Municipio.Estado.Nombre = Estado.Element("{http://schemas.datacontract.org/2004/07/ML}Nombre")?.Value ?? string.Empty;

                    result.Objects.Add(usuario);
                }
                result.Correct = true;
            }
            else
            {
                result.Correct = false;
            }

            return result;
        }

        [NonAction]

        private ML.Result GetByIdWebService(int IdUsuario)
        {
            ML.Result Result = new Result();

            string action = "http://tempuri.org/IUsuario/GetByIdWebService";
            string url = "http://localhost:48690/Usuario.svc";
            string xmlEnviar = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                                   <soapenv:Header/>
                                   <soapenv:Body>
                                      <tem:GetByIdWebService>
                                         <tem:idUsuario>{IdUsuario}</tem:idUsuario>
                                      </tem:GetByIdWebService>
                                   </soapenv:Body>
                                </soapenv:Envelope>";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("SOAPAction", action);
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Accept = "text/xml";
            request.Method = "POST";

            //enviar solicitud
            using (Stream stream = request.GetRequestStream())
            {
                byte[] content = Encoding.UTF8.GetBytes(xmlEnviar);
                stream.Write(content, 0, content.Length);  //enviar peticion
            }

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                    {
                        string xml = stream.ReadToEnd();

                        Result = deserealizarUsuario(xml);

                    }
                }
            }
            catch (Exception ex)
            {
                Result.ErrorMessage = "El registro no pudo obtenerse";

            }
            return Result;
        }

        private ML.Result deserealizarUsuario(string xml)
        {
            ML.Result result = new ML.Result();
            ML.Usuario usuario = new ML.Usuario();

            var xdoc = XDocument.Parse(xml);
            // Acceder a GetUsuarioByIdResult usando el namespace correcto
            var usuarioElement = xdoc.Descendants().FirstOrDefault(e =>
                e.Name.LocalName == "Object" &&
                e.GetDefaultNamespace().NamespaceName == "http://tempuri.org/");

            if (usuarioElement != null)
            {
                
                usuario.Rol = new Rol();
                usuario.Direccion = new Direccion();
                usuario.Direccion.Colonia = new Colonia();
                usuario.Direccion.Colonia.Municipio = new Municipio();
                usuario.Direccion.Colonia.Municipio.Estado = new Estado();

                int idUsuario;
                bool status;
                // Si el idUsuario es nulo o vacio entonces no hace la conversion, sino entonces hace la conversion y lo retorna en una variable
                int.TryParse(usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}idUsuario")?.Value, out idUsuario);
                usuario.idUsuario = idUsuario;
                bool.TryParse(usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Estatus")?.Value, out status);

                usuario.Nombre = usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Nombre")?.Value ?? string.Empty;
                usuario.ApellidoPaterno = usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}ApellidoPaterno")?.Value ?? string.Empty;
                usuario.ApellidoMaterno = usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}ApellidoMaterno")?.Value ?? string.Empty;
                usuario.Telefono = usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Telefono")?.Value ?? string.Empty;
                usuario.UserName = usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}UserName")?.Value ?? string.Empty;
                usuario.Password = usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Password")?.Value ?? string.Empty;
                usuario.FechaNacimiento = usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}FechaNacimiento")?.Value ?? string.Empty;
                usuario.Sexo = usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Sexo")?.Value ?? string.Empty;
                usuario.Celular = usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Celular")?.Value ?? string.Empty;
                usuario.Estatus = status;
                usuario.Curp = usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Curp")?.Value ?? string.Empty;
                usuario.Email = usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Email")?.Value ?? string.Empty;

                var Rol = usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Rol");
                int idRol;
                int.TryParse(Rol.Element("{http://schemas.datacontract.org/2004/07/ML}IdRol")?.Value, out idRol);
                usuario.Rol.IdRol = idRol;

                var Direccion = usuarioElement.Element("{http://schemas.datacontract.org/2004/07/ML}Direccion");
                int idDireccion;
                int.TryParse(Direccion.Element("{http://schemas.datacontract.org/2004/07/ML}IdDireccion")?.Value, out idDireccion);
                usuario.Direccion.IdDireccion = idDireccion;
                usuario.Direccion.Calle = Direccion.Element("{http://schemas.datacontract.org/2004/07/ML}Calle")?.Value ?? string.Empty;
                usuario.Direccion.NumeroInterior = Direccion.Element("{http://schemas.datacontract.org/2004/07/ML}NumeroInterior")?.Value ?? string.Empty;
                usuario.Direccion.NumeroExterior = Direccion.Element("{http://schemas.datacontract.org/2004/07/ML}NumeroExterior")?.Value ?? string.Empty;

                var Colonia = Direccion.Element("{http://schemas.datacontract.org/2004/07/ML}Colonia");
                int idColonia;
                int.TryParse(Colonia.Element("{http://schemas.datacontract.org/2004/07/ML}IdColonia")?.Value, out idColonia);
                usuario.Direccion.Colonia.IdColonia = idColonia;

                var Municipio = Colonia.Element("{http://schemas.datacontract.org/2004/07/ML}Municipio");
                int idMunicipio;
                int.TryParse(Municipio.Element("{http://schemas.datacontract.org/2004/07/ML}IdMunicipio")?.Value, out idMunicipio);
                usuario.Direccion.Colonia.Municipio.IdMunicipio = idMunicipio;

                var Estado = Municipio.Element("{http://schemas.datacontract.org/2004/07/ML}Estado");
                int idEstado;
                int.TryParse(Estado.Element("{http://schemas.datacontract.org/2004/07/ML}IdEstado")?.Value, out idEstado);
                usuario.Direccion.Colonia.Municipio.Estado.IdEstado = idEstado;

                result.Object = usuario;
                result.Correct = true;
            }
            else
            {
                result.Correct = false;
            }
            return result;
        }
    
    
        //AQUI VCA EL METODO PARA ENVIAR PETICION PARA AGREGAR O ACTUALIZAR
        private ML.Result InsertarActualizar(ML.Usuario usuario)
        {
            ML.Result result = new Result();

            string url = "http://localhost:48690/Usuario.svc"; //URL del web service de usuario
            string SoapEnviar; // se crea la variable para el XML de envio

            string action;  // Se hace la variable para colocar la peticion dependiendo su es agregar o actualizar

            if (usuario.idUsuario == 0) //SE agrega
            {
                action = "http://tempuri.org/IUsuario/AddWebService";
                SoapEnviar = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"" xmlns:ml=""http://schemas.datacontract.org/2004/07/ML"" xmlns:arr=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
                                   <soapenv:Header/>
                                   <soapenv:Body>
                                      <tem:AddWebService>
                                         <tem:usuario>
                                            <ml:ApellidoMaterno>{usuario.ApellidoMaterno}</ml:ApellidoMaterno>
                                            <ml:ApellidoPaterno>{usuario.ApellidoPaterno}</ml:ApellidoPaterno>
                                            <ml:Celular>{usuario.Celular}</ml:Celular>
                                            <ml:Curp>{usuario.Curp}</ml:Curp>
                                            <ml:Direccion>
                                               <ml:Calle>{usuario.Direccion.Calle}</ml:Calle>
                                               <ml:Colonia>
                                                  <ml:IdColonia>{usuario.Direccion.Colonia.IdColonia}</ml:IdColonia>
                                               </ml:Colonia>
                                               <ml:NumeroExterior>{usuario.Direccion.NumeroExterior}</ml:NumeroExterior>
                                               <ml:NumeroInterior>{usuario.Direccion.NumeroInterior}</ml:NumeroInterior>
                                               <ml:Usuario/>
                                            </ml:Direccion>
                                            <ml:Email>{usuario.Email}</ml:Email>
                                            <ml:FechaNacimiento>{usuario.FechaNacimiento}</ml:FechaNacimiento>
                                            <ml:Nombre>{usuario.Nombre}</ml:Nombre>
                                            <ml:Password>{usuario.Password}</ml:Password>
                                            <ml:Rol>
                                               <ml:IdRol>{usuario.Rol.IdRol}</ml:IdRol>
                                            </ml:Rol>
                                            <ml:Sexo>{usuario.Sexo}</ml:Sexo>

                                            <ml:Telefono>{usuario.Telefono}</ml:Telefono>
                                            <ml:UserName>{usuario.UserName}</ml:UserName>
                                         </tem:usuario>
                                      </tem:AddWebService>
                                   </soapenv:Body>
                                </soapenv:Envelope>";
            }
            else
            {
                action = "http://tempuri.org/IUsuario/UpdateWebService";
                SoapEnviar = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"" xmlns:ml=""http://schemas.datacontract.org/2004/07/ML"" xmlns:arr=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
                                   <soapenv:Header/>
                                   <soapenv:Body>
                                      <tem:UpdateWebService>
                                         <tem:usuario>
                                           <ml:ApellidoMaterno>{usuario.ApellidoMaterno}</ml:ApellidoMaterno>
                                            <ml:ApellidoPaterno>{usuario.ApellidoPaterno}</ml:ApellidoPaterno>
                                            <ml:Celular>{usuario.Celular}</ml:Celular>
                                            <ml:Curp>{usuario.Curp}</ml:Curp>
                                            <ml:Direccion>
                                               <ml:Calle>{usuario.Direccion.Calle}</ml:Calle>
                                               <ml:Colonia>
                                                  <ml:IdColonia>{usuario.Direccion.Colonia.IdColonia}</ml:IdColonia>
                                               </ml:Colonia>
                                               <ml:NumeroExterior>{usuario.Direccion.NumeroExterior}</ml:NumeroExterior>
                                               <ml:NumeroInterior>{usuario.Direccion.NumeroInterior}</ml:NumeroInterior>
                                               <ml:Usuario/>
                                            </ml:Direccion>
                                            <ml:Email>{usuario.Email}</ml:Email>
                                            <ml:FechaNacimiento>{usuario.FechaNacimiento}</ml:FechaNacimiento>
                                            <ml:Nombre>{usuario.Nombre}</ml:Nombre>
                                            <ml:Password>{usuario.Password}</ml:Password>
                                            <ml:Rol>
                                               <ml:IdRol>{usuario.Rol.IdRol}</ml:IdRol>
                                            </ml:Rol>
                                            <ml:Sexo>{usuario.Sexo}</ml:Sexo>

                                            <ml:Telefono>{usuario.Telefono}</ml:Telefono>
                                            <ml:UserName>{usuario.UserName}</ml:UserName>
                                            <ml:idUsuario>{usuario.idUsuario}</ml:idUsuario>
                                         </tem:usuario>
                                      </tem:UpdateWebService>
                                   </soapenv:Body>
                                </soapenv:Envelope>";
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("SOAPAction", action);
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Accept = "text/xml";
            request.Method = "POST";

            //ENVIAR PETICION
            using (Stream stream = request.GetRequestStream())
            {
                byte[] content = Encoding.UTF8.GetBytes(SoapEnviar);
                stream.Write(content, 0, content.Length);
            }

            //OBTENER LA RESPUESTA A LA PETICION
            try
            {
                using(WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string xml = reader.ReadToEnd(); // Se lee la respuesta del Web service
                        var xdoc = XDocument.Parse(xml);
                        var xresult = xdoc.Descendants().FirstOrDefault(e =>
                                        e.Name.LocalName == "Correct" &&
                                        e.GetDefaultNamespace().NamespaceName == "http://tempuri.org/"); //obtengo la etiqueta completa por su nombre

                        result.Correct = bool.Parse(xresult.Value);
                    }
                }
            }catch(Exception ex)
            {
                result.Correct = false;
            }

            return result;
        }
    }
}