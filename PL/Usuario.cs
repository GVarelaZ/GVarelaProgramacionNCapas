using ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL // PL = Presentation Layout
{
    internal class Usuario
    {
        // Metodos estaticos para que todos los proyectos pueden usarlos
        public static void Add()
        {
            ML.Usuario usuario = new ML.Usuario(); // objeto de tipo ML (estructura)

            Console.WriteLine("Ingresa tu nombre o nombres:");
            usuario.Nombre = Console.ReadLine();
            Console.WriteLine("Ingresa apellido paterno:");
            usuario.ApellidoPaterno = Console.ReadLine();
            Console.WriteLine("Ingresa apellido materno:");
            usuario.ApellidoMaterno = Console.ReadLine();
            Console.WriteLine("Ingresa tu número telefónico:");
            usuario.Telefono = Console.ReadLine();
            Console.WriteLine("Ingresa un nombre de usuario:");
            usuario.UserName = Console.ReadLine();
            Console.WriteLine("Ingresa una contraseña:");
            usuario.Password = Console.ReadLine();
            Console.WriteLine("Ingresa fecha de nacimiento (DD/MM/YYYY):");
            usuario.FechaNacimiento = Console.ReadLine();
            Console.WriteLine("Ingresa sexo (H:hombre, M:mujer):");
            usuario.Sexo = Console.ReadLine();
            Console.WriteLine("Ingresa tu numero celular:");
            usuario.Celular = Console.ReadLine();
            Console.WriteLine("Ingresa tu estatus (Activo, Inactivo):");
            string status = Console.ReadLine();
            Console.WriteLine("Ingresa tu curp:");
            usuario.Curp = Console.ReadLine();
            Console.WriteLine("Ingresa una foto de perfil:");
            usuario.Imagen = Encoding.ASCII.GetBytes(Console.ReadLine());
            usuario.Rol.IdRol = 1;
            Console.WriteLine("Ingresa un correo electronico:");
            usuario.Email = Console.ReadLine();

            if (status.ToLower() == "activo")
            {
                usuario.Estatus = Convert.ToBoolean("true");
            }
            else
            {
                usuario.Estatus = Convert.ToBoolean("false");
            }

            ML.Result result = BL.Usuario.AddEF(usuario); // Metodo que llega desde BL para agregar usuario con RESULT
            //ML.Result result = BL.Usuario.AddLINQ(usuario); //Metodo de insertar con LINQ

            if (result.Correct)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            else
            {
                Console.WriteLine(result.ErrorMessage);
            }
        }

        public static void Delete()
        {
            Console.WriteLine("Ingresa el ID del cliente que deseas eliminar:");
            int idUsuario = Convert.ToInt32(Console.ReadLine());

            ML.Result result = BL.Usuario.DeleteEF(idUsuario); // Metodo que llega desde BL para eliminar usuario
            //Result result = BL.Usuario.GetByIdLINQ(idUsuario);

            if (result.Correct)
            {
                Console.WriteLine(result.ErrorMessage);
                ML.Usuario usuario = (ML.Usuario)result.Object;
                Console.WriteLine("Id: " + usuario.idUsuario);
                Console.WriteLine("Nombre: " + usuario.Nombre);
                Console.WriteLine("Apellido paterno: " + usuario.ApellidoPaterno);
                Console.WriteLine("Apellido materno: " + usuario.ApellidoMaterno);
                Console.WriteLine("Telefono: " + usuario.Telefono);
                Console.WriteLine("UserName: " + usuario.UserName);
                Console.WriteLine("Password: " + usuario.Password);
                Console.WriteLine("Fecha de nacimiento: " + usuario.FechaNacimiento);
                Console.WriteLine("Sexo: " + usuario.Sexo);
                Console.WriteLine("Celular: " + usuario.Celular);
                Console.WriteLine("CURP: " + usuario.Curp);
                Console.WriteLine("Imagen: " + usuario.Imagen);
                Console.WriteLine("IdRol: " + usuario.Rol.IdRol);
                Console.WriteLine("Email: " + usuario.Email);

                if (usuario.Estatus)
                {
                    Console.WriteLine("Estado: Activo");
                }
                else
                {
                    Console.WriteLine("Estado: Inactivo");
                }

                Console.WriteLine("\n");

                Console.WriteLine("--------------------------------------------------------------");
                Console.Write("Aun deseas eliminarlo? SI ó NO: ");
                string opcion = Console.ReadLine();
                if (opcion.ToLower() == "si")
                {
                    ML.Result resultado = BL.Usuario.DeleteEF(idUsuario);
                    Console.WriteLine(resultado.ErrorMessage);

                }
                else
                {
                    Console.WriteLine("Saliendo del sistema, registro NO eliminado...!");
                }

            }
            else
            {
                Console.WriteLine(result.ErrorMessage);

            }

        }

        //CON RESULT
        public static void Change()
        {
            ML.Usuario usuario = new ML.Usuario();

            Console.WriteLine("Ingresa el ID del usuario que deseas actualizar:");
            usuario.idUsuario = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Ingresa tu nombre o nombres:");
            usuario.Nombre = Console.ReadLine();
            Console.WriteLine("Ingresa apellido paterno:");
            usuario.ApellidoPaterno = Console.ReadLine();
            Console.WriteLine("Ingresa apellido materno:");
            usuario.ApellidoMaterno = Console.ReadLine();
            Console.WriteLine("Ingresa tu número telefónico:");
            usuario.Telefono = Console.ReadLine();
            Console.WriteLine("Ingresa un nombre de usuario:");
            usuario.UserName = Console.ReadLine();
            Console.WriteLine("Ingresa una contraseña:");
            usuario.Password = Console.ReadLine();
            Console.WriteLine("Ingresa fecha de nacimiento (dd/mm/yyyy):");
            usuario.FechaNacimiento = Console.ReadLine();
            Console.WriteLine("Ingresa sexo (H:hombre, M:mujer):");
            usuario.Sexo = Console.ReadLine();
            Console.WriteLine("Ingresa tu numero celular:");
            usuario.Celular = Console.ReadLine();
            Console.WriteLine("Ingresa tu estatus (Activo, Inactivo):");
            string status = Console.ReadLine();
            Console.WriteLine("Ingresa tu curp:");
            usuario.Curp = Console.ReadLine();
            Console.WriteLine("Ingresa una foto de perfil:");
            usuario.Imagen = Encoding.ASCII.GetBytes(Console.ReadLine());
            usuario.Rol.IdRol = 1;
            Console.WriteLine("Ingresa un correo electronico:");
            usuario.Email = Console.ReadLine();

            if (status.ToLower() == "activo")
            {
                usuario.Estatus = Convert.ToBoolean("true");
            }
            else
            {
                usuario.Estatus = Convert.ToBoolean("false");
            }

            ML.Result result = BL.Usuario.ChangeEF(usuario);  //El result cacha el return result en un result
            //ML.Result result = BL.Usuario.ChangeLINQ(usuario);

            if (result.Correct)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            else
            {
                Console.WriteLine(result.ErrorMessage);
            }

        }

        public static void Get()
        {
            Console.WriteLine("Ingresa que deseas realizar:\n" +
                "1. Monstrar todos los registros \n" + "2. Mostrar solo 1 registro");
            int opcion = Convert.ToInt32(Console.ReadLine());

            switch (opcion)
            {
                case 1:
                    ML.Result result = new Result(); //Regresa todos los registros de la tabla
                    //ML.Result result = BL.Usuario.GetAllLINQ();

                    if (result.Correct)
                    {
                        Console.WriteLine(result.ErrorMessage + "\n\n");
                        foreach (ML.Usuario usuario in result.Objects)
                        {
                            Console.WriteLine("Id: " + usuario.idUsuario);
                            Console.WriteLine("Nombre: " + usuario.Nombre);
                            Console.WriteLine("Apellido paterno: " + usuario.ApellidoPaterno);
                            Console.WriteLine("Apellido materno: " + usuario.ApellidoMaterno);
                            Console.WriteLine("Telefono: " + usuario.Telefono);
                            Console.WriteLine("UserName: " + usuario.UserName);
                            Console.WriteLine("Password: " + usuario.Password);
                            Console.WriteLine("Fecha de nacimiento: " + usuario.FechaNacimiento);
                            Console.WriteLine("Sexo: " + usuario.Sexo);
                            Console.WriteLine("Celular: " + usuario.Celular);
                            Console.WriteLine("CURP: " + usuario.Curp);
                            Console.WriteLine("Imagen: " + usuario.Imagen);
                            Console.WriteLine("IdRol: " + usuario.Rol.IdRol);
                            Console.WriteLine("Email: " + usuario.Email);

                            if (usuario.Estatus)
                            {
                                Console.WriteLine("Estado: Activo");
                            }
                            else
                            {
                                Console.WriteLine("Estado: Inactivo");
                            }

                            Console.WriteLine("\n");

                        }
                    }
                    else
                    {
                        Console.WriteLine("Hubo un error " + result.ErrorMessage);
                    }
                    break;
                case 2:
                    Console.WriteLine("Ingresa el ID del cliente que deseas consultar:");
                    int idUsuario = Convert.ToInt32(Console.ReadLine());

                    ML.Result Result = BL.Usuario.GetByIdEF(idUsuario); //Regresa solo el registro del ID ingresada
                    //ML.Result Result = BL.Usuario.GetByIdLINQ(idUsuario);

                    if (Result.Correct)
                    {
                        Console.WriteLine(Result.ErrorMessage + "\n\n");
                        ML.Usuario usuario = (ML.Usuario)Result.Object;
                        Console.WriteLine("Id: " + usuario.idUsuario);
                        Console.WriteLine("Nombre: " + usuario.Nombre);
                        Console.WriteLine("Apellido paterno: " + usuario.ApellidoPaterno);
                        Console.WriteLine("Apellido materno: " + usuario.ApellidoMaterno);
                        Console.WriteLine("Telefono: " + usuario.Telefono);
                        Console.WriteLine("UserName: " + usuario.UserName);
                        Console.WriteLine("Password: " + usuario.Password);
                        Console.WriteLine("Fecha de nacimiento: " + usuario.FechaNacimiento);
                        Console.WriteLine("Sexo: " + usuario.Sexo);
                        Console.WriteLine("Celular: " + usuario.Celular);
                        Console.WriteLine("CURP: " + usuario.Curp);
                        Console.WriteLine("Imagen: " + usuario.Imagen);
                        Console.WriteLine("IdRol: " + usuario.Rol.IdRol);
                        Console.WriteLine("Email: " + usuario.Email);

                        if (usuario.Estatus)
                        {
                            Console.WriteLine("Estado: Activo");
                        }
                        else
                        {
                            Console.WriteLine("Estado: Inactivo");
                        }

                        Console.WriteLine("\n");

                    }
                    else
                    {
                        Console.WriteLine("Hubo un error" + Result.ErrorMessage);
                    }
                    break;
                default:
                    Console.WriteLine("Respuesta incorrecta");
                    break;
            }
        }

        public static void cargaMasiva()
        {
            ML.Result result = new ML.Result();

            Console.WriteLine("Entrando a carga masiva");
            string ruta = @"C:\Users\digis\Downloads\pruebaCargaMasiva.txt"; //Se coloca la ruta absoluta del txt que deseamos leer
            try
            {
                StreamReader sr = new StreamReader(ruta); //Instanciamos un objeto de tipo StreamReader para poder leer el archivo
                string fila = ""; 
                while((fila =  sr.ReadLine()) != null){ //condicional while, para que vaya iterando fila por fila hasta que se encuentre con alguna que no contenga nada o este vacia
                    Console.WriteLine(fila);
                }
            }
            catch (Exception ex)
            {
            }
        } 
    }
}
