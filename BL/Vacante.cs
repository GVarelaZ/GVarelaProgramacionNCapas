using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Vacante
    {
        public static Result VacantesGetAll()
        {

            Result result = new Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    var query = (from vacantes in context.Vacantes
                                 select new
                                 {
                                     idVacante = vacantes.IdVacante,
                                     nombre = vacantes.Nombre
                                 }).ToList();

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var vacanteBD in query)
                        {
                            ML.Vacante vacante = new ML.Vacante();
                            vacante.IdVacante = vacanteBD.idVacante;
                            vacante.Nombre = vacanteBD.nombre;
                            result.Objects.Add(vacante);
                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception e)
            {
                result.Correct = false;
                result.ErrorMessage = e.Message;
                result.ex = e;

            }
            return result;
        }

    }
}
