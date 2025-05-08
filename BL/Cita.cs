using DL_EF;
using ML;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
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
                    //int filasAfectadas = context.insertarCita(Convert.ToDateTime(candidato.cita.FechaHora), candidato.cita.piso.IdPiso, candidato.IdCandidato, candidato.cita.estatusCita.IdEstatusCita);

                    DL_EF.Cita cita = new DL_EF.Cita();
                    cita.FechaHora = DateTime.Parse(candidato.cita.FechaHora);
                    cita.IdPiso = ((byte?)candidato.cita.piso.IdPiso);
                    cita.IdCandidato = candidato.IdCandidato;
                    cita.IdEstatusCita = (byte?)candidato.cita.estatusCita.IdEstatusCita;

                    context.Citas.Add(cita);

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

        public static Result ObtenerCita(int IdCandidato)
        {
            Result result = new Result();

            try
            {
                using (GVarelaProgramacionNCapasEntities context = new GVarelaProgramacionNCapasEntities())
                {
                    var citaBD = context.CitasGetById(IdCandidato).SingleOrDefault();

                    if (citaBD != null)
                    {
                        ML.Candidato candidatoML = new ML.Candidato();
                        candidatoML.cita = new ML.Cita();
                        candidatoML.cita.piso = new ML.Piso();
                        candidatoML.cita.estatusCita = new ML.EstatusCita();
                        candidatoML.carrera = new ML.Carrera();

                        candidatoML.cita.IdCita = citaBD.IdCita == null ? 0 : citaBD.IdCita.Value;
                        candidatoML.cita.FechaHora = Convert.ToString(citaBD.FechaHora);
                        candidatoML.cita.piso.IdPiso = citaBD.IdPiso == null ? 0 : citaBD.IdPiso.Value;
                        candidatoML.cita.estatusCita.IdEstatusCita = citaBD.IdEstatusCita == null ? 0 : citaBD.IdEstatusCita.Value;
                        candidatoML.IdCandidato = citaBD.IdCandidato;
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

        public static Result ObtenerCandidato(int IdCandidato)
        {
            Result result = new Result();

            try
            {
                using (DL_EF.GVarelaProgramacionNCapasEntities context = new GVarelaProgramacionNCapasEntities())
                {
                    var candidatoBD = (from candidato in context.Candidatoes
                                       where candidato.IdCandidato == IdCandidato
                                       join cita in context.Citas
                                       on candidato.IdCandidato equals cita.IdCandidato into CitaCandidato
                                       from cita in CitaCandidato.DefaultIfEmpty()
                                       select new
                                       {
                                           Id = candidato.IdCandidato,
                                           Nombre = candidato.Nombre,
                                           ApellidoPaterno = candidato.ApellidoPaterno,
                                           ApellidoMaterno = candidato.ApellidoMaterno,
                                           Correo = candidato.Correo,
                                           Telefono = candidato.Telefono,
                                           Foto = candidato.Foto,
                                           CarreraNombre = candidato.Carrera.Nombre,
                                           idCita = cita.IdCita == cita.IdCita ? cita.IdCandidato : 0,
                                       }).SingleOrDefault();

                    if (candidatoBD != null)
                    {
                        ML.Candidato candidatoML = new ML.Candidato();
                        candidatoML.carrera = new ML.Carrera();
                        candidatoML.cita = new ML.Cita();

                        candidatoML.IdCandidato = candidatoBD.Id;
                        candidatoML.Nombre = candidatoBD.Nombre;
                        candidatoML.ApellidoPaterno = candidatoBD.ApellidoPaterno;
                        candidatoML.ApellidoMaterno = candidatoBD.ApellidoMaterno;
                        candidatoML.Correo = candidatoBD.Correo;
                        candidatoML.Telefono = candidatoBD.Telefono;
                        candidatoML.Foto = candidatoBD.Foto;
                        candidatoML.carrera.Nombre = candidatoBD.CarreraNombre;
                        candidatoML.cita.IdCita = (int)(candidatoBD.idCita == null ? 0 : candidatoBD.idCita);

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