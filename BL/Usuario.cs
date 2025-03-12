using DL_EF;
using ML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BL  // BL = bussiness Layout  (reglas del negocio)
{
    public class Usuario
    {
        public static Result AddEF(ML.Usuario usuario) // Usando EntityFramework
        {
            ML.Result result = new ML.Result();

            try
            {       // Utiliza la conexion del model de la base de datos con Entity Framework

                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                // Contiene la cadena de conexion directa del modelo echo en EntityFramework
                {
                    int rowsAffects = context.UsuarioAdd(usuario.Nombre, usuario.ApellidoPaterno, usuario.ApellidoMaterno, usuario.Telefono,
                        usuario.UserName, usuario.Password, Convert.ToDateTime(usuario.FechaNacimiento),
                        usuario.Sexo, usuario.Celular, usuario.Estatus,
                        usuario.Curp, usuario.Imagen, usuario.Rol.IdRol, usuario.Email, usuario.Direccion.Calle,
                        usuario.Direccion.NumeroInterior, usuario.Direccion.NumeroExterior, usuario.Direccion.Colonia.IdColonia);
                    // EntityFramework convierte los StoredProcedure en metodos y solo falta agregar los parametros a insertar

                    if (rowsAffects == 2)
                    {
                        result.Correct = true;
                        result.ErrorMessage = "Registro insertado con exito";
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al agregar el registro";
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

        public static Result DeleteEF(int idUsario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    int rowsAffected = context.UsuarioDelete(idUsario);

                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                        result.ErrorMessage = "\n El registro a sido eliminado";
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "\n El registro no se pudo eliminar";

                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message + "LA CONEXION A FALLADO";
                result.ex = ex;
            }

            return result;
        }

        public static Result ChangeEF(ML.Usuario usuario)
        {
            Result result = new Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    int rowsAffected = context.UsuarioUpdate(usuario.idUsuario, usuario.Nombre, usuario.ApellidoPaterno, usuario.ApellidoMaterno,
                        usuario.Telefono, usuario.UserName, usuario.Password, DateTime.Parse(usuario.FechaNacimiento),
                        usuario.Sexo, usuario.Celular,
                        usuario.Estatus, usuario.Curp, usuario.Imagen, usuario.Rol.IdRol, usuario.Email, usuario.Direccion.IdDireccion,
                        usuario.Direccion.Calle, usuario.Direccion.NumeroInterior, usuario.Direccion.NumeroExterior, usuario.Direccion.Colonia.IdColonia);

                    if (rowsAffected == 2)
                    {
                        result.Correct = true;
                        result.ErrorMessage = "\nRegistro actualizado con existo...!";
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "\nA ocurrido un error al actualizar...!";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message + "\nLA CONEXION A FALLADO";
                result.ex = ex;
            }

            return result;
        }

        public static Result GetAllEF(ML.Usuario usuario)
        {
            Result result = new Result(); // Modelo de Result para el retorno de informacion

            try
            {

                using (GVarelaProgramacionNCapasEntities context = new GVarelaProgramacionNCapasEntities())
                {

                    var registros = context.UsuarioGetAll(usuario.Nombre,usuario.ApellidoPaterno, usuario.ApellidoMaterno,
                                                          //usuario.Rol.IdRol).ToList(); //Retorna una lista con los valores obtenidos

                    //var registros = context.UsuarioGetAllViewSP(usuario.Nombre, usuario.ApellidoPaterno, usuario.ApellidoMaterno,
                                                                usuario.Rol.IdRol).ToList();  //SP consumiendo una vista

                    if (registros.Count > 0)
                    {
                        result.Objects = new List<object>(); //Crea una lista de modelos Objects

                        foreach (var data in registros) // Recorrido de las tablas por filas
                        {
                            ML.Usuario usuarioML = new ML.Usuario(); //Se crea un objeto para cada iteracion o fila
                            usuarioML.Rol = new ML.Rol(); //  Se crea una instancia desde usuario, se abre la puerta
                            usuarioML.Direccion = new ML.Direccion();
                            usuarioML.Direccion.Colonia = new ML.Colonia();
                            usuarioML.Direccion.Colonia.Municipio = new ML.Municipio();
                            usuarioML.Direccion.Colonia.Municipio.Estado = new ML.Estado();

                            usuarioML.idUsuario = Convert.ToInt32(data.IdUsuario.ToString());
                            usuarioML.Nombre = data.UsuarioNombre;
                            usuarioML.ApellidoPaterno = data.ApellidoPaterno;
                            usuarioML.ApellidoMaterno = data.ApellidoMaterno;
                            usuarioML.Telefono = data.Telefono;
                            usuarioML.UserName = data.UserName;
                            usuarioML.Password = data.Password;
                            usuarioML.FechaNacimiento = data.FechaNacimiento;
                            usuarioML.Sexo = data.Sexo;
                            usuarioML.Celular = data.Celular;
                            usuarioML.Estatus = Convert.ToBoolean(data.Status.ToString());
                            usuarioML.Curp = data.CURP;
                            usuarioML.Imagen = data.Imagen;
                            usuarioML.Email = data.Email;
                            usuarioML.Rol.Nombre = data.RolNombre;
                            if (data.IdDireccion != null)
                            {
                                usuarioML.Direccion.IdDireccion = data.IdDireccion.Value;
                            }
                            else
                            {
                                usuarioML.Direccion.IdDireccion = 0;
                            }
                            usuarioML.Direccion.Calle = data.Calle;
                            usuarioML.Direccion.NumeroInterior = data.NumeroInterior;
                            usuarioML.Direccion.NumeroExterior = data.NumeroExterior;
                            usuarioML.Direccion.Colonia.Nombre = data.ColoniaNombre;
                            usuarioML.Direccion.Colonia.CodigoPostal = data.CodigoPostal;
                            usuarioML.Direccion.Colonia.Municipio.Nombre = data.MunicipioNombre;
                            usuarioML.Direccion.Colonia.Municipio.Estado.Nombre = data.EstadoNOmbre;

                            result.Objects.Add(usuarioML); // Se ingresa un objeto por cada iteracion
                        }

                        result.Correct = true;
                        result.ErrorMessage = "Campos obtenidos correctamente";

                    }
                    else
                    {
                        // No hay registros
                        result.Correct = false;
                        result.ErrorMessage = "\nNo hay registros o datos";
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

        public static Result GetByIdEF(int idUsuario) // Selecciona solo 1 registro
        {
            ML.Result result = new ML.Result(); // Crea un modelo de result

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {

                    var datos = context.UsuarioGetById(idUsuario).SingleOrDefault();

                    if (datos != null)
                    {
                        //var datos = registro.Single(); // solo recibe los datos de una fila de la tabla

                        ML.Usuario usuario = new ML.Usuario(); //Se crea un objeto para cada iteracion o fila
                        usuario.Rol = new ML.Rol(); //  Se crea una instancia desde usuario, se abre la puerta
                        usuario.Direccion = new ML.Direccion();
                        usuario.Direccion.Colonia = new ML.Colonia();
                        usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                        usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();


                        usuario.idUsuario = Convert.ToInt32(datos.IdUsuario);
                        usuario.Nombre = datos.UsuarioNombre;
                        usuario.ApellidoPaterno = datos.ApellidoPaterno;
                        usuario.ApellidoMaterno = datos.ApellidoMaterno;
                        usuario.Telefono = datos.Telefono;
                        usuario.UserName = datos.UserName;
                        usuario.Password = datos.Password;
                        usuario.FechaNacimiento = datos.FechaNacimiento;
                        usuario.Sexo = datos.Sexo;
                        usuario.Celular = datos.Celular;
                        usuario.Estatus = Convert.ToBoolean(datos.Status);
                        usuario.Curp = datos.CURP;
                        usuario.Imagen = datos.Imagen;
                        usuario.Email = datos.Email;
                        if (datos.IdRol != null)
                        {
                            usuario.Rol.IdRol = datos.IdRol.Value;

                        }
                        else
                        {
                            usuario.Rol.IdRol = 0;
                        }
                        if (datos.IdDireccion != null)
                        {
                            usuario.Direccion.IdDireccion = datos.IdDireccion.Value;

                        }
                        else
                        {
                            usuario.Direccion.IdDireccion = 0;
                        }
                        
                        usuario.Direccion.Calle = datos.Calle;
                        usuario.Direccion.NumeroInterior = datos.NumeroInterior;
                        usuario.Direccion.NumeroExterior = datos.NumeroExterior;
                        if (datos.IdColonia != null)
                        {
                            usuario.Direccion.Colonia.IdColonia = datos.IdColonia.Value;

                        }
                        else
                        {
                            usuario.Direccion.Colonia.IdColonia = 0;
                        }
                        if (datos.IdMunicipio != null)
                        {
                            usuario.Direccion.Colonia.Municipio.IdMunicipio = datos.IdMunicipio.Value;

                        }
                        else
                        {
                            usuario.Direccion.Colonia.Municipio.IdMunicipio = 0;
                        }
                        if (datos.IdEstado != null)
                        {
                            usuario.Direccion.Colonia.Municipio.Estado.IdEstado = datos.IdEstado.Value;

                        }
                        else
                        {
                            usuario.Direccion.Colonia.Municipio.Estado.IdEstado = 0;
                        }

                        result.Object = usuario; //Se guarda solo un modelo de Object 

                        result.Correct = true;
                        result.ErrorMessage = "Registro obtenido correctamente";
                    }

                    else
                    {
                        // No hay registros
                        result.Correct = false;
                        result.ErrorMessage = "\n No hay registros o datos";
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n LA CONEXION A FALLADO" + ex);
            }

            return result;
        }

        public static Result GetStatus(int IdUsuario, bool Status)
        {
            Result result = new Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new GVarelaProgramacionNCapasEntities())
                {
                    int rowsAffected = context.UsuarioUpdateStatus(IdUsuario, Status);

                    if (rowsAffected > 0)
                    {

                        result.Correct = true;
                        result.ErrorMessage = "Estado cambiado con exito";
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

        public static Result UsuarioUpdateAddDireccion(ML.Usuario usuarioD)
        {
            Result result = new Result();

            try
            {
                using (GVarelaProgramacionNCapasEntities context = new GVarelaProgramacionNCapasEntities())
                {
                    int rowsAffected = context.UsuarioUpdateDireccionAdd(usuarioD.idUsuario, usuarioD.Nombre, usuarioD.ApellidoPaterno, usuarioD.ApellidoMaterno,
                        usuarioD.Telefono, usuarioD.UserName, usuarioD.Password, DateTime.Parse(usuarioD.FechaNacimiento),
                        usuarioD.Sexo, usuarioD.Celular,
                        usuarioD.Estatus, usuarioD.Curp, usuarioD.Imagen, usuarioD.Rol.IdRol, usuarioD.Email,
                        usuarioD.Direccion.Calle, usuarioD.Direccion.NumeroInterior, usuarioD.Direccion.NumeroExterior, usuarioD.Direccion.Colonia.IdColonia);

                    if (rowsAffected == 2)
                    {

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

                    if(tabla.Rows.Count > 0) {
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
                            int estatus = int.Parse(valores[9].ToString()); //el tipo booleano se convierte en int para guardarlo en el modelo
                            if (estatus == 1)
                            {
                                usuario.Estatus = Convert.ToBoolean(estatus); //si es 1 el valor se convierte a booleano
                            }
                            else
                            {
                                usuario.Estatus = Convert.ToBoolean(estatus); //si no el 0 se convierte en booleano
                            }
                            usuario.Curp = valores[10].ToString();
                            usuario.Rol.IdRol = int.Parse(valores[12].ToString());
                            usuario.Email = valores[13].ToString();
                            usuario.Direccion.Calle = valores[14].ToString();
                            usuario.Direccion.NumeroInterior = valores[15].ToString();
                            usuario.Direccion.NumeroExterior = valores[16].ToString();
                            usuario.Direccion.Colonia.IdColonia = int.Parse(valores[17].ToString());
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
                result.Correct=false;
                result.ErrorMessage = ex.Message;
                result.ex = ex;
            }

            return result;
        }
        /*  
          public static Result AddLINQ(ML.Usuario usuario)
          {
              ML.Result result = new ML.Result();

              using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
              {
                  DL_EF.Usuario usuarioBD = new DL_EF.Usuario(); // instanciamos un objeto de tipo DL_EF (modelo)
                  usuarioBD.Nombre = usuario.Nombre;
                  usuarioBD.ApellidoPaterno = usuario.ApellidoPaterno;
                  usuarioBD.ApellidoMaterno = usuario.ApellidoMaterno;
                  usuarioBD.Telefono = usuario.Telefono;
                  usuarioBD.UserName = usuario.UserName;
                  usuarioBD.Password = usuario.Password;
                  usuarioBD.FechaNacimiento = DateTime.Parse(usuario.FechaNacimiento.ToString());
                  usuarioBD.Sexo = usuario.Sexo;
                  usuarioBD.Celular = usuario.Celular;
                  usuarioBD.Status = usuario.Estatus;
                  usuarioBD.CURP = usuario.Curp;
                  usuarioBD.Imagen = usuario.Imagen;
                  usuarioBD.IdRol = usuario.IdRol;
                  usuarioBD.Email = usuario.Email;
                  //Pasar los valores de las proedades del ML.Usuario al DL_EF.UsuarioBD

                  context.Usuarios.Add(usuarioBD); //Generando el query de insert
                  int rowsAffected = context.SaveChanges(); //Ejecuta el query de insert

                  if (rowsAffected > 0)
                  {
                      result.Correct = true;
                      result.ErrorMessage = "\nRegistro guardado exitosamente...!";

                  }
                  else
                  {
                      result.Correct = false;
                      result.ErrorMessage = "\nRegistro no se guardo...!";
                  }
              }
              return result;
          }

          public static Result DeleteLINQ(int idUsuario)
          {
              Result result = new Result();
              try
              {
                  using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                  {
                      //Paso 1. Hacer un select con LINQ
                      var selectOnlyDelete = (from usuario in context.Usuarios
                                              where usuario.IdUsuario == idUsuario
                                              select usuario).SingleOrDefault();

                      if (selectOnlyDelete != null) // si la consulta no llega vacia
                      {
                          context.Usuarios.Remove(selectOnlyDelete); //Genera el query de DELETE usuario where id = ?;

                          int rowsAffected = context.SaveChanges(); //Ejecuta el query de DELETE

                          if (rowsAffected > 0) // si se realizo correctamente
                          {
                              result.Correct = true;
                              result.ErrorMessage = "Registro eliminado con exito";
                          }
                          else
                          {
                              result.Correct = false;
                              result.ErrorMessage = "Error al eliminar registro";
                          }

                      }
                      else
                      {
                          result.Correct = false;
                          result.ErrorMessage = "Registro no econtrado, no se puede eliminar...!";
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
          public static Result ChangeLINQ(ML.Usuario usuario)
          {
              Result result = new Result();
              try
              {
                  using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                  {
                      DL_EF.Usuario usuarioBD = new DL_EF.Usuario();

                      /*var selectChange = (from usuarios in context.Usuarios
                                          where usuarios.IdUsuario == usuario.idUsuario
                                          select new
                                          {
                                              usuarios.Nombre,
                                              usuarios.ApellidoPaterno,
                                              usuarios.ApellidoMaterno,
                                              usuarios.Telefono,
                                              usuarios.UserName,
                                              usuarios.Password,
                                              usuarios.FechaNacimiento,
                                              usuarios.Sexo,
                                              usuarios.Celular,
                                              usuarios.Status,
                                              usuarios.CURP,
                                              usuarios.Imagen,
                                              usuarios.IdRol,
                                              usuario.Email
                                          }).AsEnumerable().Select(z => new DL_EF.Usuario
                                          {
                                              Nombre = z.Nombre,
                                              ApellidoPaterno = z.ApellidoPaterno,
                                              ApellidoMaterno = z.ApellidoMaterno,
                                              Telefono = z.Telefono,
                                              UserName = z.UserName,
                                              Password = z.Password,
                                              FechaNacimiento = z.FechaNacimiento,
                                              Sexo = z.Sexo,
                                              Celular = z.Celular,
                                              Status = z.Status,
                                              CURP = z.CURP,
                                              Imagen = z.Imagen,
                                              IdRol = z.IdRol,
                                              Email = z.Email

                                          }).SingleOrDefault();

                      //Realiza una consulta de SELECT para traer los valores y/o propiedades que van a ser cambiadas
                      var selectChange = (from UsuarioBD in context.Usuarios
                                          where UsuarioBD.IdUsuario == usuario.idUsuario
                                          select UsuarioBD).SingleOrDefault();


                      if (selectChange != null)
                      {
                          //Si trae registros
                          selectChange.Nombre = usuario.Nombre;
                          selectChange.ApellidoPaterno = usuario.ApellidoPaterno;
                          selectChange.ApellidoMaterno = usuario.ApellidoMaterno;
                          selectChange.Telefono = usuario.Telefono;
                          selectChange.UserName = usuario.UserName;
                          selectChange.Password = usuario.Password;
                          selectChange.FechaNacimiento = Convert.ToDateTime(usuario.FechaNacimiento);
                          selectChange.Sexo = usuario.Sexo;
                          selectChange.Celular = usuario.Celular;
                          selectChange.Status = usuario.Estatus;
                          selectChange.CURP = usuario.Curp;
                          selectChange.Imagen = usuario.Imagen;
                          selectChange.IdRol = usuario.IdRol;
                          selectChange.Email = usuario.Email;
                          // El valor de la variable anterior se cambia por el valor que el usuario ingreso en la capa PL.

                          int rowsAffected = context.SaveChanges();

                          if (rowsAffected > 0)
                          {
                              result.Correct = true;
                              result.ErrorMessage = "\nRegistro actualizado correctamente...!";
                          }
                          else
                          {
                              result.Correct = false;
                              result.ErrorMessage = "\nNo se pudo actualizar registro...ª";
                          }
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

          /*
          public static Result GetAllLINQ()
          {
              Result result = new Result();
              try
              {
                  using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                  {
                      /*var select = (from usuarios in context.Usuarios
                                    select new
                                    {
                                        usuarios.IdUsuario,
                                        usuarios.Nombre,
                                        usuarios.ApellidoPaterno,
                                        usuarios.ApellidoMaterno,
                                        usuarios.Telefono,
                                        usuarios.UserName,
                                        usuarios.Password,
                                        usuarios.FechaNacimiento,
                                        usuarios.Sexo,
                                        usuarios.Celular,
                                        usuarios.Status,
                                        usuarios.CURP,
                                        usuarios.Imagen,
                                        usuarios.IdRol,
                                        usuarios.Email
                                    }).AsEnumerable().Select(z => new DL_EF.Usuario
                                    {
                                        IdUsuario = z.IdUsuario,
                                        Nombre = z.Nombre,
                                        ApellidoPaterno = z.ApellidoPaterno,
                                        ApellidoMaterno = z.ApellidoMaterno,
                                        Telefono = z.Telefono,
                                        UserName = z.UserName,
                                        Password = z.Password,
                                        FechaNacimiento = z.FechaNacimiento,
                                        Sexo = z.Sexo,
                                        Celular = z.Celular,
                                        Status = z.Status,
                                        CURP = z.CURP,
                                        Imagen = z.Imagen,
                                        IdRol = z.IdRol,
                                        Email = z.Email

                                    }).ToList();

                      var select = (from usuarioBD in context.Usuarios
                                    select new
                                    {
                                        IdUsuario = usuarioBD.IdUsuario,
                                        Nombre = usuarioBD.Nombre,
                                        ApellidoPaterno = usuarioBD.ApellidoPaterno,
                                        ApellidoMaterno = usuarioBD.ApellidoMaterno,
                                        Telefono = usuarioBD.Telefono,
                                        UserName = usuarioBD.UserName,
                                        Password = usuarioBD.Password,
                                        FechaNacimiento = usuarioBD.FechaNacimiento,
                                        Sexo = usuarioBD.Sexo,
                                        Celular = usuarioBD.Celular,
                                        Status = usuarioBD.Status,
                                        CURP = usuarioBD.CURP,
                                        Imagen = usuarioBD.Imagen,
                                        IdRol = usuarioBD.IdRol,
                                        Email = usuarioBD.Email
                                    }).ToList();

                      //SELECT (todas o algunas variables) FROM Usuario, es la consulta que se realiza con LINQ

                      if (select.Count > 0)
                      {
                          result.Objects = new List<object>(); // se guarda cada registro en una lista

                          foreach (var usuario in select) //Se hace un recorrido por todos los registros y se guardan en la lista
                          {
                              ML.Usuario usuarioML = new ML.Usuario();
                              usuarioML.idUsuario = usuario.IdUsuario;
                              usuarioML.Nombre = usuario.Nombre;
                              usuarioML.ApellidoPaterno = usuario.ApellidoPaterno;
                              usuarioML.ApellidoMaterno = usuario.ApellidoMaterno;
                              usuarioML.Telefono = usuario.Telefono;
                              usuarioML.UserName = usuario.UserName;
                              usuarioML.Password = usuario.Password;
                              usuarioML.FechaNacimiento = usuario.FechaNacimiento.ToString("dd/MM/yyyy");
                              usuarioML.Sexo = usuario.Sexo;
                              usuarioML.Celular = usuario.Celular;
                              usuarioML.Estatus = usuario.Status;
                              usuarioML.Curp = usuario.CURP;
                              usuarioML.Imagen = usuario.Imagen;
                              if (usuario.IdRol == null)
                              {
                                  usuarioML.IdRol = 0;
                              }
                              else
                              {
                                  usuarioML.IdRol = (int)usuario.IdRol;
                              }
                              usuarioML.Email = usuario.Email;

                              result.Objects.Add(usuarioML); //Se ingresan esos registros en la lista, como si fueran objetos
                          }

                          result.Correct = true;
                          result.ErrorMessage = "\nRegistros obtuvimos con exito";
                      }
                      else
                      {
                          result.Object = false;
                          result.ErrorMessage = "\nRegistros no obtuvidos";
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
          public static Result GetByIdLINQ(int idUsuario)
          {
              Result result = new Result();
              try
              {
                  using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                  {

                      var resultGetById = (from usuarioBD in context.Usuarios
                                              where usuarioBD.IdUsuario == idUsuario
                                           select new
                                           {
                                               IdUsuario = usuarioBD.IdUsuario,
                                               Nombre = usuarioBD.Nombre,
                                               ApellidoPaterno = usuarioBD.ApellidoPaterno,
                                               ApellidoMaterno = usuarioBD.ApellidoMaterno,
                                               Telefono = usuarioBD.Telefono,
                                               UserName = usuarioBD.UserName,
                                               Password = usuarioBD.Password,
                                               FechaNacimiento = usuarioBD.FechaNacimiento,
                                               Sexo = usuarioBD.Sexo,
                                               Celular = usuarioBD.Celular,
                                               Status = usuarioBD.Status,
                                               CURP = usuarioBD.CURP,
                                               Imagen = usuarioBD.Imagen,
                                               IdRol = usuarioBD.IdRol,
                                               Email = usuarioBD.Email
                                           }).SingleOrDefault();

                      if(resultGetById != null)
                      {
                          ML.Usuario usuarioML = new ML.Usuario();
                          usuarioML.idUsuario = resultGetById.IdUsuario;
                          usuarioML.Nombre = resultGetById.Nombre;
                          usuarioML.ApellidoPaterno = resultGetById.ApellidoPaterno;
                          usuarioML.ApellidoMaterno = resultGetById.ApellidoMaterno;
                          usuarioML.Telefono = resultGetById.Telefono;
                          usuarioML.UserName = resultGetById.UserName;
                          usuarioML.Password = resultGetById.Password;
                          usuarioML.FechaNacimiento = resultGetById.FechaNacimiento.ToString();
                          usuarioML.Sexo = resultGetById.Sexo;
                          usuarioML.Celular = resultGetById.Celular;
                          usuarioML.Estatus = resultGetById.Status;
                          usuarioML.Curp = resultGetById.CURP;
                          usuarioML.Imagen = resultGetById.Imagen;
                          if (resultGetById.IdRol == null)
                          {
                              usuarioML.IdRol = 0;
                          }
                          else
                          {
                              usuarioML.IdRol = (int)resultGetById.IdRol;

                          }

                          usuarioML.Email = resultGetById.Email;

                          result.Object = usuarioML; //Aqui solo se instancia un objeto porque solo se espera que llegue un registro en esta consulta
                          result.Correct = true;
                          result.ErrorMessage = "\nRegistro obtenido exitosamente...!";
                      }
                      else
                      {
                          result.Correct = false;
                          result.ErrorMessage = "\nRegistro no encontrado...!";

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
          }*/
    }
}