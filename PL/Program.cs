using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class Program
    {
        static void Main(string[] args)

        {
            int opcion;
            do
            {
                Console.WriteLine("\nBIENVENIDO QUE DESEAS REALIZAR");
                Console.WriteLine("1. Registrar un nuevo registro");
                Console.WriteLine("2. Eliminar algun registro");
                Console.WriteLine("3. Actualizar datos de algun registro");
                Console.WriteLine("4. Obtener registros");
                Console.WriteLine("6. Carga masiva");
                Console.WriteLine("5. Salir \n");
                opcion = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("-----------------------------------------------------------------------");
                switch (opcion)
                {
                    case 1:
                        Usuario.Add();  // Realiza la inserccion de los datos
                        break;
                    case 2:
                        Usuario.Delete(); // Realiza la eliminacion de los datos
                        break;
                    case 3:
                        Usuario.Change();  // Realiza la actualizacion de los datos
                        break;
                    case 4:
                        Usuario.Get(); // Realiza una consulta de todos o solo 1 registro
                        break;
                    case 5:
                        Console.WriteLine("Hasta luego...!");
                        break;
                    case 6:
                        Usuario.cargaMasiva();
                        break;
                    default:
                        Console.WriteLine("Opcion incorrecta, vuelve a intentar...");
                        break;
                }
            }
            while (opcion != 5);
        }
    }
}
