using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Carrera
    {
        public static Result GetAllCarreras()
        {
            Result result = new Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    var Carreras = (from carrera in context.Carreras
                                    select new
                                    {
                                        Id = carrera.IdCarrera,
                                        nombre = carrera.Nombre
                                    }).ToList();

                    if (Carreras.Count > 0) {
                        result.Objects = new List<object>();

                        foreach(var carrera in Carreras)
                        {
                            ML.Carrera carreraML = new ML.Carrera();
                            carreraML.IdCarrera = carrera.Id;
                            carreraML.Nombre = carrera.nombre;
                            result.Objects.Add(carreraML);
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
