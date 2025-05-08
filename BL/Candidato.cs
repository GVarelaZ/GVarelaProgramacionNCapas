using DL_EF;
using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Candidato
    {

        public static Result candidatoGetAll(int IdVacante)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    var candidatos = context.ObtenerCandidatos(IdVacante).ToList();

                    if (candidatos.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var candidato in candidatos)
                        {
                            ML.Candidato candidatoML = new ML.Candidato();
                            candidatoML.vacante = new ML.Vacante();
                            candidatoML.bolsaTrabajo = new ML.BolsaTrabajo();
                            candidatoML.carrera = new ML.Carrera();
                            candidatoML.universidad = new ML.Universidad();
                            candidatoML.promedio = new ML.Promedio();

                            candidatoML.IdCandidato = candidato.IdCandidato;
                            candidatoML.Nombre = candidato.Nombre;
                            candidatoML.ApellidoPaterno = candidato.ApellidoPaterno;
                            candidatoML.ApellidoMaterno = candidato.ApellidoMaterno;
                            candidatoML.Edad = candidato.Edad;
                            candidatoML.Correo = candidato.Correo;
                            candidatoML.Telefono = candidato.Telefono;
                            candidatoML.Direccion = candidato.Direccion;
                            candidatoML.Foto = candidato.Foto;
                            candidatoML.Curriculum = candidato.Curriculum;
                            candidatoML.universidad.IdUniveridad = candidato.IdUniversidad;
                            candidatoML.universidad.Nombre = candidato.NombreUniversidad;
                            candidatoML.carrera.IdCarrera = candidato.IdCarrera;
                            candidatoML.carrera.Nombre = candidato.NombreCarrera;
                            candidatoML.bolsaTrabajo.IdBolsaTrabajo = candidato.IdBolsaTrabajo;
                            candidatoML.bolsaTrabajo.Nombre = candidato.NombreBolsaTrabajo;
                            candidatoML.vacante.IdVacante = candidato.IdVacante;
                            candidatoML.vacante.Nombre = candidato.NombreVacante;
                            candidatoML.promedio.IdPromedio = candidato.IdPromedio == null ? 0 : candidato.IdPromedio.Value;
                            candidatoML.promedio.PromedioUniversidad = candidato.PromedioUniversidad;
                            candidatoML.promedio.PromedioPreparatoria = candidato.PromedioPreparatoria;
                            candidatoML.promedio.PromedioSecundaria = candidato.PromedioSecundaria;

                            result.Objects.Add(candidatoML);
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

        public static Result deleteCandidatos(int IdCandidato)
        {
            Result result = new Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    int rowsEffected = context.deleteCandidato(IdCandidato);

                    if (rowsEffected == 2)
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

        public static Result getByIdCandidato(int IdCandidato)
        {
            Result result = new Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new DL_EF.GVarelaProgramacionNCapasEntities())
                {
                    var candidatoBD = context.getByIdCandidato(IdCandidato).SingleOrDefault();

                    if (candidatoBD != null)
                    {
                        ML.Candidato candidatoML = new ML.Candidato();
                        candidatoML.vacante = new ML.Vacante();
                        candidatoML.bolsaTrabajo = new ML.BolsaTrabajo();
                        candidatoML.carrera = new ML.Carrera();
                        candidatoML.universidad = new ML.Universidad();
                        candidatoML.promedio = new ML.Promedio();

                        candidatoML.IdCandidato = candidatoBD.IdCandidato;
                        candidatoML.Nombre = candidatoBD.Nombre;
                        candidatoML.ApellidoPaterno = candidatoBD.ApellidoPaterno;
                        candidatoML.ApellidoMaterno = candidatoBD.ApellidoMaterno;
                        candidatoML.Edad = candidatoBD.Edad;
                        candidatoML.Correo = candidatoBD.Correo;
                        candidatoML.Telefono = candidatoBD.Telefono;
                        candidatoML.Direccion = candidatoBD.Direccion;
                        candidatoML.Foto = candidatoBD.Foto;
                        candidatoML.Curriculum = candidatoBD.Curriculum;
                        candidatoML.universidad.IdUniveridad = candidatoBD.IdUniversidad == null ? 0 : candidatoBD.IdUniversidad.Value;
                        candidatoML.carrera.IdCarrera = candidatoBD.IdCarrera == null ? 0 : candidatoBD.IdCarrera.Value;
                        candidatoML.carrera.Nombre = candidatoBD.CarreraNombre;
                        candidatoML.bolsaTrabajo.IdBolsaTrabajo = candidatoBD.IdBolsaTrabajo == null ? 0 : candidatoBD.IdBolsaTrabajo.Value;
                        candidatoML.vacante.IdVacante = candidatoBD.IdVacante == null ? 0 : candidatoBD.IdVacante.Value;
                        candidatoML.promedio.IdPromedio = candidatoBD.IdPromedio;
                        candidatoML.promedio.PromedioUniversidad = candidatoBD.PromedioUniversidad;
                        candidatoML.promedio.PromedioPreparatoria = candidatoBD.PromedioPreparatoria;
                        candidatoML.promedio.PromedioSecundaria = candidatoBD.PromedioSecundaria;

                        result.Object = candidatoML;

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

        public static Result agregarCandidato(ML.Candidato candidato)
        {
            Result result = new Result();

            try
            {
                using (GVarelaProgramacionNCapasEntities context = new GVarelaProgramacionNCapasEntities())
                {
                    int candidatoAdd = context.CandidatoAdd(candidato.Nombre, candidato.ApellidoPaterno, candidato.ApellidoMaterno, candidato.Edad,
                        candidato.Correo, candidato.Telefono, candidato.Direccion, candidato.Foto, candidato.Curriculum, candidato.universidad.IdUniveridad,
                        candidato.carrera.IdCarrera, candidato.bolsaTrabajo.IdBolsaTrabajo, candidato.vacante.IdVacante, candidato.promedio.PromedioUniversidad,
                        candidato.promedio.PromedioPreparatoria, candidato.promedio.PromedioSecundaria);

                    if (candidatoAdd == 2)
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

        public static Result actualizarCandidato(ML.Candidato candidato)
        {
            Result result = new Result();

            try
            {
                using (GVarelaProgramacionNCapasEntities context = new GVarelaProgramacionNCapasEntities())
                {
                    int rowsAffected = context.CandidatoUpdate(candidato.IdCandidato, candidato.Nombre, candidato.ApellidoPaterno, candidato.ApellidoMaterno, candidato.Edad,
                        candidato.Correo, candidato.Telefono, candidato.Direccion, candidato.Foto, candidato.Curriculum, candidato.universidad.IdUniveridad,
                        candidato.carrera.IdCarrera, candidato.bolsaTrabajo.IdBolsaTrabajo, candidato.vacante.IdVacante, candidato.promedio.IdPromedio, candidato.promedio.PromedioUniversidad,
                        candidato.promedio.PromedioPreparatoria, candidato.promedio.PromedioSecundaria);

                    if (rowsAffected > 0)
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
    }
}
