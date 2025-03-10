using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Municipio
    {
        public static ML.Result MunicipioGetByIdEstado(int idEstado)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    var municipioBD = (from municipio in context.Municipios
                                       where municipio.IdEstado == idEstado
                                       select new
                                       {
                                           idMunicipio = municipio.IdMunicipio,
                                           nombre = municipio.Nombre,
                                       }).ToList();

                    if(municipioBD.Count() > 0)
                    {
                        result.Objects = new List<object>();

                        foreach(var municipiosBD in municipioBD)
                        {
                            ML.Municipio municipio= new ML.Municipio();
                            municipio.IdMunicipio = municipiosBD.idMunicipio;
                            municipio.Nombre = municipiosBD.nombre;
                            result.Objects.Add(municipio);
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
    }
}
