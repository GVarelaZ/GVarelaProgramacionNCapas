using DL_EF;
using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class BolsaTrabajo
    {
        public static Result GetAllBolsaTrabajo()
        {
            Result result = new Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    var listaBolsas = (from bolsaTrabajo in context.BolsaTrabajoes
                                       select new
                                       {
                                           Id = bolsaTrabajo.IdBolsaTrabajo,
                                           nombre = bolsaTrabajo.Nombre
                                       }).ToList();

                    if (listaBolsas.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var bolsa in listaBolsas)
                        {
                            ML.BolsaTrabajo bolsaTrabajo = new ML.BolsaTrabajo();
                            bolsaTrabajo.IdBolsaTrabajo = bolsa.Id;
                            bolsaTrabajo.Nombre = bolsa.nombre;
                            result.Objects.Add(bolsaTrabajo);
                        }

                        result.Correct = true;
                    }
                    else { result.Correct = false; }
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
