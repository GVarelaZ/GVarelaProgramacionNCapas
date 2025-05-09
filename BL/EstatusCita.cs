using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class EstatusCita
    {
        public static ML.Result obtenerEstatusCita()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    var EstatusCita = (from estatus in context.EstatusCitas
                                       select estatus).ToList();

                    if (EstatusCita != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var estatus in EstatusCita)
                        {
                            ML.EstatusCita estatusML = new ML.EstatusCita();
                            estatusML.IdEstatusCita = estatus.IdEstatusCita;
                            estatusML.Nombre = estatus.Nombre;
                            result.Objects.Add(estatusML);

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