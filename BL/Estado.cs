using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Estado
    {
        public static ML.Result EstadoGetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    //var EstadosBD = context.UsuarioGetAllEstados(); EF
                    var EstadosBD = (from estados in context.Estadoes
                                     select new
                                     {
                                         idEstado = estados.IdEstado,
                                         nombre = estados.Nombre
                                     }).ToList();   //LINQ

                    if (EstadosBD.Count() > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var estados in EstadosBD)
                        {

                            ML.Estado estado = new ML.Estado();
                            estado.IdEstado = estados.idEstado;
                            estado.Nombre = estados.nombre;
                            result.Objects.Add(estado);
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
