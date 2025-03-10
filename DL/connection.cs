using System;
using System.Configuration;

namespace DL
{
    public class connection /* clase public para que todos los proyectos tengan acceso
                                a ella */
    {

        public static string GetConnection() // Metodo para establecer una conexion
        {
            string connection = ConfigurationManager.ConnectionStrings
                ["GVarelaProgramacionNCapas"].ToString();
            //string connection = ConfigurationManager.ConnectionStrings["GvarelaNCapas2"].ToString();
                //Permite traer ConnectionString desde el app.config convertido a string
            return connection; // regresa el connectionString 
        }
    }
}
