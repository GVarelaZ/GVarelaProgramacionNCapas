using ML;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Excel
    {
        public static Result LeerExcel(string cadenaConexion)
        {
            Result result = new Result();

            try
            {
                using (OleDbConnection context = new OleDbConnection(cadenaConexion)) //se crea la conexion con el proveedor OLEDB
                {
                    OleDbCommand cmd = new OleDbCommand(); //OLEDBCommand para leer el archivo

                    cmd.CommandText = "SELECT * FROM [Hoja1$]"; // vamos a consultar la hoja1
                    cmd.Connection = context;

                    context.Open();

                    OleDbDataAdapter adapter = new OleDbDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataTable tabla = new DataTable();
                    adapter.Fill(tabla);

                    if (tabla.Rows.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (DataRow valores in tabla.Rows)
                        {
                            ML.Usuario usuario = new ML.Usuario();
                            usuario.Rol = new ML.Rol();
                            usuario.Direccion = new ML.Direccion();
                            usuario.Direccion.Colonia = new ML.Colonia();

                            usuario.Nombre = valores[0].ToString();
                            usuario.ApellidoPaterno = valores[1].ToString();
                            usuario.ApellidoMaterno = valores[2].ToString();
                            usuario.Telefono = valores[3].ToString();
                            usuario.UserName = valores[4].ToString();
                            usuario.Password = valores[5].ToString();
                            usuario.FechaNacimiento = valores[6].ToString();
                            usuario.Sexo = valores[7].ToString();
                            usuario.Celular = valores[8].ToString();

                            if (valores[9].ToString() == "")
                            {
                                //estatus = false; //el tipo booleano se convierte en int para guardarlo en el modelo
                                usuario.Estatus = false; //si es 1 el valor se convierte a booleano
                            }
                            else
                            {
                                usuario.Estatus = true; //si no el 0 se convierte en booleano
                            }
                            usuario.Curp = valores[10].ToString();
                            if (valores[12].ToString() == "")
                            {
                                usuario.Rol.IdRol = 0;

                            }
                            else
                            {
                                usuario.Rol.IdRol = int.Parse(valores[12].ToString());
                            }
                            usuario.Email = valores[13].ToString();
                            usuario.Direccion.Calle = valores[14].ToString();
                            usuario.Direccion.NumeroInterior = valores[15].ToString();
                            usuario.Direccion.NumeroExterior = valores[16].ToString();
                            if (valores[17].ToString() == "")
                            {
                                usuario.Rol.IdRol = 0;

                            }
                            else
                            {
                                usuario.Direccion.Colonia.IdColonia = int.Parse(valores[17].ToString());
                            }
                            
                            result.Objects.Add(usuario);

                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;

                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.ex = ex;
            }

            return result;
        }

        public static Result ValidarExcel(List<object> registros) //Metodo para validar si el campo viene nulo o esta vacio
        {
            Result resultEx = new Result(); // se creo un nuevo modelo para guardar el numero de registro y los errores

            resultEx.Objects = new List<object>(); //se crea una nueva lista vacia para que cada nuevo registro almacene sus errores
            
            int contador = 1;

            foreach (ML.Usuario usuario in registros) //se almacena cada registro en un modelo de usuario para poder acceder a las propiedades
            {
                ExcelResult resultExcel = new ExcelResult();
                resultExcel.NumeroRegistro = contador;
                if (usuario.Nombre == "" || usuario.Nombre == null || usuario.Nombre.Count() > 30) //validacion solo si son vacios, nulos o son mayores a algo
                {
                    resultExcel.ErrorMessage += "No existe nombre, favor de revisar el archivo |"; // se contatena el mensaje de error
                }
                if (usuario.ApellidoPaterno == "" || usuario.ApellidoPaterno == null || usuario.ApellidoPaterno.Count() > 30)
                {
                    resultExcel.ErrorMessage += "El nombre no tiene un apellido paterno, favor de verificar |";
                }
                if (usuario.ApellidoMaterno == "" || usuario.ApellidoMaterno == null || usuario.ApellidoMaterno.Count() > 30)
                {
                    resultExcel.ErrorMessage += "El nombre no tiene un apellido materno, favor de verificar |";
                }
                if (usuario.Telefono == "" || usuario.Telefono == null || usuario.Telefono.Count() > 10)
                {
                    resultExcel.ErrorMessage += "No se ha encontrado un numero de telefono Y/O solo debe de ser de 10 digitos |";
                }
                if (usuario.UserName == "" || usuario.UserName == null || usuario.UserName.Count() > 30)
                {
                    resultExcel.ErrorMessage += "Debe de contener un userName y este dede de ser unico |";
                }
                if (usuario.Password == "" || usuario.Password == null || usuario.Password.Count() > 30)
                {
                    resultExcel.ErrorMessage += "Debe de existir una contraseña para ingresar al sistema, favor de colocar una |";
                }
                if (usuario.FechaNacimiento == "" || usuario.FechaNacimiento == null)
                {
                    resultExcel.ErrorMessage += "No se ha encontrado la fecha de nacimiento, favor de verificar |";
                }
                if (usuario.Sexo == "" || usuario.Sexo == null || usuario.Sexo.Count() > 2)
                {
                    resultExcel.ErrorMessage += "El sexo se debe de colocar como H(hombre) o M(mujer), verificalo |";
                }
                if (usuario.Celular == "" || usuario.Celular == null || usuario.Celular.Count() > 10)
                {
                    resultExcel.ErrorMessage += "No se ha encontrado un numero de celular Y/O solo debe de ser de 10 digitos |";
                }

                int estatus = Convert.ToInt16(usuario.Estatus);
                if (estatus > 1 || estatus.ToString() == null)
                    resultExcel.ErrorMessage += "El estatus no puede venir vacio y/o solo puede contener 1(activo) o 0(inactivo) |";

                resultExcel.ErrorMessage += usuario.Curp == "" || usuario.Curp == null || usuario.Curp.Count() > 18 ? "El curp es necesario, favor de colocarlo |" : "";

                resultExcel.ErrorMessage += usuario.Imagen != null ? "La imagen se puede colocar como NULL |" : "";

                resultExcel.ErrorMessage += usuario.Rol.IdRol == 0 || usuario.Rol.IdRol.ToString() == null || usuario.Rol.IdRol > 7 ? "El id del rol no puedde venir en vacio y/o solo existen del 1 al 6 |" : "";

                resultExcel.ErrorMessage += usuario.Email == "" || usuario.Email == null || usuario.Email.Count() > 25 ? "El correo debe de ser unico |" : "";

                resultExcel.ErrorMessage += usuario.Direccion.Calle == "" || usuario.Direccion.Calle == null || usuario.Direccion.
                    Calle.Count() > 25 ? "La calle es necesario para su registro, favor de verificar \n" : "";

                resultExcel.ErrorMessage += usuario.Direccion.NumeroInterior == "" || usuario.Direccion.NumeroInterior == null ||
                    usuario.Direccion.NumeroInterior.Count() > 10 ? "no hay un numero interior asociado con el domicilio del usuario |" : "";

                resultExcel.ErrorMessage += usuario.Direccion.NumeroExterior == "" || usuario.Direccion.NumeroExterior == null ||
                    usuario.Direccion.NumeroExterior.Count() > 10 ? "no hay un numero exterior asociado con el domicilio del usuario |" : "";

                resultExcel.ErrorMessage += usuario.Direccion.Colonia.IdColonia == 0 || usuario.Direccion.Colonia.IdColonia.ToString() == null ||
                    usuario.Direccion.Colonia.IdColonia > 3000 ? "El id de la colonia no puede venir vacio o no existe |" : "";

                if (resultExcel.ErrorMessage != "")
                {
                    //si ocurrieron errores
                    resultEx.Objects.Add(resultExcel);
                }

                contador++;
            }

            return resultEx;
        }
    }
}
