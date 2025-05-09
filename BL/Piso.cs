using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Piso
    {
        public static Result obtenerPisos()
        {
            Result result = new Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    var pisosBD = (from piso in context.Pisoes
                                   select piso).ToList();

                    if( pisosBD.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach( var piso in pisosBD)
                        {
                            ML.Piso pisoML = new ML.Piso();
                            pisoML.IdPiso = piso.IdPiso;
                            pisoML.Nombre = piso.Nombre;
                            result.Objects.Add(pisoML);
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
