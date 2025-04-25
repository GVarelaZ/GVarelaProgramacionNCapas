using DL_EF;
using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Cita
    {
        public static Result obtenerCandidatosCita(int IdVacante)
        {

            Result result = new Result();

            try
            {
                using (GVarelaProgramacionNCapasEntities context = new GVarelaProgramacionNCapasEntities())
                {
                    var Candidatos = context.obtenerCitas(IdVacante).ToList();

                    if (Candidatos.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var candidato in Candidatos)
                        {
                            ML.Candidato candidatoML = new ML.Candidato();
                            candidatoML.vacante = new ML.Vacante();
                            candidatoML.bolsaTrabajo = new ML.BolsaTrabajo();
                            candidatoML.carrera = new ML.Carrera();
                            candidatoML.universidad = new ML.Universidad();
                            candidatoML.promedio = new ML.Promedio();
                            candidatoML.cita = new ML.Cita();

                            candidatoML.IdCandidato = candidato.IdCandidato;
                            candidatoML.cita.IdCita = candidato.IdCita == null ? 0 : candidato.IdCita.Value;
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
            catch (Exception e)
            {
                result.Correct = false;
                result.ErrorMessage = e.Message;
                result.ex = e;
            }

            return result;
        }

        public static Result agregarCita(ML.Candidato candidato)
        {
            Result result = new Result();

            try
            {
                using (GVarelaProgramacionNCapasEntities context = new GVarelaProgramacionNCapasEntities())
                {
                    int filasAfectadas = context.insertarCita(Convert.ToDateTime(candidato.cita.FechaHora), candidato.cita.piso.IdPiso, candidato.IdCandidato, candidato.cita.estatusCita.IdEstatusCita);

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
            catch (Exception e)
            {
                result.Correct = false;
                result.ErrorMessage = e.Message;
                result.ex = e;
            }
            return result;
        }

        public static Result actualizarCita(ML.Candidato candidato)
        {
            Result result = new Result();

            try
            {
                using (GVarelaProgramacionNCapasEntities context = new GVarelaProgramacionNCapasEntities())
                {
                    int filasAfectadas = context.updateCita(candidato.cita.IdCita, Convert.ToDateTime(candidato.cita.FechaHora), candidato.cita.piso.IdPiso, candidato.IdCandidato, candidato.cita.estatusCita.IdEstatusCita);

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
            catch (Exception e)
            {
                result.Correct = false;
                result.ErrorMessage = e.Message;
                result.ex = e;
            }
            return result;
        }

        public static Result obtenerCita(ML.Candidato candidato)
        {
            Result result = new Result();

            try
            {
                using (GVarelaProgramacionNCapasEntities context = new GVarelaProgramacionNCapasEntities())
                {
                    var citaBD = context.CitasGetById(candidato.IdCandidato, candidato.cita.IdCita).SingleOrDefault();

                    if (citaBD != null)
                    {
                        ML.Candidato candidatoML = new ML.Candidato();
                        candidato.cita = new ML.Cita();

                        candidatoML.cita.IdCita = citaBD.IdCita;
                        candidatoML.cita.FechaHora = Convert.ToString(citaBD.FechaHora);
                        candidatoML.cita.piso.IdPiso = citaBD.IdPiso == null ? 0 : citaBD.IdPiso.Value;
                        candidatoML.cita.estatusCita.IdEstatusCita = citaBD.IdEstatusCita == null ? 0 : citaBD.IdEstatusCita.Value;
                        candidatoML.IdCandidato = citaBD.IdCandidato == null ? 0 : citaBD.IdCandidato.Value;
                        candidatoML.Nombre = citaBD.Nombre;
                        candidatoML.ApellidoPaterno = citaBD.ApellidoPaterno;
                        candidatoML.ApellidoMaterno = citaBD.ApellidoMaterno;
                        candidatoML.Correo = citaBD.Correo;
                        candidatoML.Telefono = citaBD.Telefono;
                        candidatoML.Foto = citaBD.Foto;
                        candidatoML.carrera.Nombre = citaBD.Carrera;
                        result.Object = candidatoML;
                        
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