using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Rol
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    var query = (from roles in context.Rols
                                select new
                                {
                                    idRol = roles.IdRol,
                                    nombre = roles.Nombre,
                                }).ToList();

                    if (query.Count > 0)
                    {

                        result.Objects = new List<object>();

                        foreach (var role in query)
                        {
                            ML.Rol rol = new ML.Rol();
                            rol.IdRol = role.idRol;
                            rol.Nombre = role.nombre;
                            result.Objects.Add(rol);
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
