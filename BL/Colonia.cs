using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Colonia
    {
        public static ML.Result ColoniaGetByIdMunicipio(int idMunicipio)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    var coloniasBD = (from colonia in context.Colonias
                                      where colonia.IdMunicipio == idMunicipio
                                      select new
                                      {
                                          idColonia = colonia.IdColonia,
                                          nombre = colonia.Nombre
                                      }).ToList();

                    if (coloniasBD.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var colonia in coloniasBD)
                        {
                            ML.Colonia coloniaEF = new ML.Colonia();
                            coloniaEF.IdColonia = colonia.idColonia;
                            coloniaEF.Nombre = colonia.nombre;
                            result.Objects.Add(coloniaEF);
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
