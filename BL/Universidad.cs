using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Universidad
    {
        public static Result GetallUniversidades()
        {
            Result result = new Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    var Universidades = (from universidad in context.Universidads
                                         select new
                                         {
                                             id = universidad.IdUniversidad,
                                             nombre = universidad.Nombre
                                         }).ToList();

                    if (Universidades.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var uni in Universidades)
                        {
                            ML.Universidad universidad = new ML.Universidad();
                            universidad.IdUniveridad = uni.id;
                            universidad.Nombre = uni.nombre;
                            result.Objects.Add(universidad);
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
