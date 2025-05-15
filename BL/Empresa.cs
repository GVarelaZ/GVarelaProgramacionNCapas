using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Empresa
    {
        public static Result AgregarEmpresa(ML.Empresa empresa)
        {
            Result result = new Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    DL_EF.Empresa empresaDL = new DL_EF.Empresa();

                    empresaDL.Nombre = empresa.Nombre;
                    empresaDL.Latitud = empresa.Latitud;
                    empresaDL.Longitud = empresa.Longitud;

                    context.Empresas.Add(empresaDL);

                    int filasAfectadas = context.SaveChanges();

                    if (filasAfectadas > 0)
                    {
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

        public static Result ActualizarEmpresa(ML.Empresa empresa)
        {
            Result result = new Result();
            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    var selectEmpresa = (from empresaDL in context.Empresas
                                         where empresaDL.IdEmpresa == empresa.IdEmpresa
                                         select empresaDL).SingleOrDefault();

                    if (selectEmpresa != null)
                    {
                        selectEmpresa.IdEmpresa = empresa.IdEmpresa;
                        selectEmpresa.Nombre = empresa.Nombre;
                        selectEmpresa.Latitud = empresa.Latitud;
                        selectEmpresa.Longitud = empresa.Longitud;

                        int rowAffected = context.SaveChanges();

                        if (rowAffected > 0)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                        }
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

        public static Result EliminarEmpresa(int IdEmpresa)
        {
            Result result = new Result();
            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    var empresaSelect = (from empresaDL in context.Empresas
                                         where empresaDL.IdEmpresa == IdEmpresa
                                         select empresaDL).SingleOrDefault();

                    if (empresaSelect != null)
                    {
                        context.Empresas.Remove(empresaSelect);

                        int rowAffected = context.SaveChanges();
                        if (rowAffected > 0)
                        {

                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                        }
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

        public static Result ObtenerEmpresas()
        {
            Result result = new Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    var empresas = (from empresaML in context.Empresas
                                    select new
                                    {
                                        id = empresaML.IdEmpresa,
                                        nombre = empresaML.Nombre,
                                        latitud = empresaML.Latitud,
                                        longitud = empresaML.Longitud,
                                    }).ToList();

                    if (empresas.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var empresa in empresas)
                        {
                            ML.Empresa empresaML = new ML.Empresa();
                            empresaML.IdEmpresa = empresa.id;
                            empresaML.Nombre = empresa.nombre;
                            empresaML.Latitud = empresa.latitud;
                            empresaML.Longitud = empresa.longitud;
                            result.Objects.Add(empresaML);
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

        public static Result ObtenerEmpresa(int IdEmpresa)
        {
            Result result = new Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    var empresaSelect = (from empresa in context.Empresas
                                         where empresa.IdEmpresa == IdEmpresa
                                         select new
                                         {
                                             id = empresa.IdEmpresa,
                                             nombre = empresa.Nombre,
                                             latitud = empresa.Latitud,
                                             longitud = empresa.Longitud,
                                         }).SingleOrDefault();

                    if (empresaSelect != null)
                    {
                        ML.Empresa empresa = new ML.Empresa();
                        empresa.IdEmpresa = empresaSelect.id;
                        empresa.Nombre = empresaSelect.nombre;
                        empresa.Latitud = empresaSelect.latitud;
                        empresa.Longitud = empresaSelect.longitud;
                        result.Object = empresa;

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

