using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using ACS.OperacionBDD;
using CONS = ACS.Constantes;
using System.Data.SqlClient;
using ACS.Models;

namespace ACS.Controllers
{
    public class PartidoController : ApiController
    {
        [Route("api/partido/crear")]
        [HttpPost]
        public HttpResponseMessage crear(PPartido ppartido_model)
        {

            Operacion objBdd = new Operacion();
            int filas_afectadas = -1;
            Response objResponse;

            DateTime hora_inicio = DateTime.Parse(ppartido_model.hora_inicio);
            DateTime hora_fin = DateTime.Parse(ppartido_model.hora_fin);

            try
            {

                List<SqlParameter> parametros = new List<SqlParameter>
                    {
                        new SqlParameter() { ParameterName= "@sede_id", Value = ppartido_model.sede_id, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@hora_inicio", Value = hora_inicio, SqlDbType = SqlDbType.DateTime },
                        new SqlParameter() { ParameterName= "@hora_fin", Value = hora_fin, SqlDbType = SqlDbType.DateTime },
                        new SqlParameter() { ParameterName= "@equipo_1_id", Value = ppartido_model.equipo_1_id, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@equipo_2_id", Value = ppartido_model.equipo_2_id, SqlDbType = SqlDbType.Int },
                    };

                filas_afectadas = objBdd.update_insertDataSp(CONS.Constantes.SP_Crear_Partido, parametros);

                if (filas_afectadas < 1)
                {
                    objResponse = new Response()
                    {
                        mensaje = CONS.Constantes.ERROR_mensaje,
                        error = CONS.Constantes.ERROR_error
                    };

                    return new HttpResponseMessage
                    {
                        Content = new ObjectContent<Response>(objResponse, Configuration.Formatters.JsonFormatter),
                        StatusCode = HttpStatusCode.OK
                    };
                }
            }
            catch (Exception)
            {
                objResponse = new Response()
                {
                    mensaje = CONS.Constantes.ERROR_mensaje,
                    error = CONS.Constantes.ERROR_error
                };

                return new HttpResponseMessage
                {
                    Content = new ObjectContent<Response>(objResponse, Configuration.Formatters.JsonFormatter),
                    StatusCode = HttpStatusCode.OK
                };
            }

            objResponse = new Response
            {
                mensaje = CONS.Constantes.OK_mensaje_POST,
                error = CONS.Constantes.OK_error_POST
            };

            return new HttpResponseMessage
            {
                Content = new ObjectContent<Response>(objResponse, Configuration.Formatters.JsonFormatter),
                StatusCode = HttpStatusCode.OK
            };
        }
        [Route("api/partido/obtener")]
        [HttpGet]
        public HttpResponseMessage obtener()
        {
            List<EXPAPartidosPorSede> objPartidos = new List<EXPAPartidosPorSede>();
            Operacion objBdd = new Operacion();
            DataTable resultado = new DataTable();

            try
            {
                resultado = objBdd.getDataSp(CONS.Constantes.SP_Obtener_Partidos);

                if (resultado != null)
                {
                    foreach (DataRow item in resultado.Rows)
                    {
                        DataTable equipos = new DataTable();
                        List<SqlParameter> parametros_equipos = new List<SqlParameter>
                                {
                                    new SqlParameter() { ParameterName= "@id", Value = int.Parse(item["id"].ToString()), SqlDbType = SqlDbType.Int },
                                };

                        equipos = objBdd.getDataSp(CONS.Constantes.SP_Obtener_Detalle_Equipo, parametros_equipos);

                        List<EXPADetalleEquipo> detalleEquipos = new List<EXPADetalleEquipo>();

                        if (equipos != null)
                        {
                            foreach (DataRow equipo in equipos.Rows)
                            {
                                detalleEquipos.Add(
                                    new EXPADetalleEquipo
                                    {
                                        EXPAID = int.Parse(equipo["EXPAID"].ToString()),
                                        nombre = equipo["nombre"].ToString(),
                                        equipo_goles = int.Parse(equipo["equipo_goles"].ToString()),
                                        grupo = equipo["grupo"].ToString(),

                                    }
                                );
                            }
                        }

                        objPartidos.Add(
                            new EXPAPartidosPorSede
                            {
                                id = int.Parse(item["id"].ToString()),
                                hora_inicio = DateTime.Parse(item["hora_inicio"].ToString()),
                                hora_fin = DateTime.Parse(item["hora_fin"].ToString()),
                                jugado = int.Parse(item["jugado"].ToString()),
                                equipos = detalleEquipos
                            }
                            );
                    }
                }
                else
                {
                    var objResponse = new Response()
                    {
                        mensaje = CONS.Constantes.PARTIDOS_No_Existen,
                        error = CONS.Constantes.ERROR_error
                    };
                    return new HttpResponseMessage
                    {
                        Content = new ObjectContent<Response>(objResponse, Configuration.Formatters.JsonFormatter),
                        StatusCode = HttpStatusCode.OK
                    };
                }

                resultado.Clear();
            }
            catch (Exception ex)
            {
                var objResponse = new Response
                {
                    mensaje = CONS.Constantes.ERROR_mensaje,
                    error = CONS.Constantes.ERROR_error
                };

                return new HttpResponseMessage
                {
                    Content = new ObjectContent<Response>(objResponse, Configuration.Formatters.JsonFormatter),
                    StatusCode = HttpStatusCode.OK
                };

            }

            return new HttpResponseMessage
            {
                Content = new ObjectContent<List<EXPAPartidosPorSede>>(objPartidos, Configuration.Formatters.JsonFormatter),
                StatusCode = HttpStatusCode.OK
            };
        }

        [Route("api/partido/obtener/{partido_id}")]
        [HttpGet]
        public HttpResponseMessage obtener(int partido_id)
        {
            EXPAPartidosPorSede objPartido = new EXPAPartidosPorSede();
            Operacion objBdd = new Operacion();
            DataTable resultado = new DataTable();

            try
            {

                List<SqlParameter> parametros = new List<SqlParameter>
                    {
                        new SqlParameter() { ParameterName= "@id", Value = partido_id, SqlDbType = SqlDbType.Int }
                    };
                resultado = objBdd.getDataSp(CONS.Constantes.SP_Obtener_Partido, parametros);

                if (resultado != null)
                {

                    foreach (DataRow item in resultado.Rows)
                    {
                        DataTable equipos = new DataTable();
                        List<SqlParameter> parametros_equipos = new List<SqlParameter>
                                {
                                    new SqlParameter() { ParameterName= "@id", Value = int.Parse(item["id"].ToString()), SqlDbType = SqlDbType.Int },
                                };

                        equipos = objBdd.getDataSp(CONS.Constantes.SP_Obtener_Detalle_Equipo, parametros_equipos);

                        List<EXPADetalleEquipo> detalleEquipos = new List<EXPADetalleEquipo>();

                        if (equipos != null)
                        {
                            foreach (DataRow equipo in equipos.Rows)
                            {
                                detalleEquipos.Add(
                                    new EXPADetalleEquipo
                                    {
                                        equipo_id = int.Parse(equipo["equipo_id"].ToString()),
                                        EXPAID = int.Parse(equipo["EXPAID"].ToString()),
                                        nombre = equipo["nombre"].ToString(),
                                        equipo_goles = int.Parse(equipo["equipo_goles"].ToString()),
                                        grupo = equipo["grupo"].ToString(),

                                    }
                                );
                            }
                        }

                        objPartido = new EXPAPartidosPorSede()
                        {
                            sede_id = int.Parse(item["sede_id"].ToString()),
                            id = int.Parse(item["id"].ToString()),
                            hora_inicio = DateTime.Parse(item["hora_inicio"].ToString()),
                            hora_fin = DateTime.Parse(item["hora_fin"].ToString()),
                            jugado = int.Parse(item["jugado"].ToString()),
                            equipos = detalleEquipos
                        };
                    }
                }
                else
                {
                    var objResponse = new Response()
                    {
                        mensaje = CONS.Constantes.PARTIDOS_No_Existen,
                        error = CONS.Constantes.ERROR_error
                    };
                    return new HttpResponseMessage
                    {
                        Content = new ObjectContent<Response>(objResponse, Configuration.Formatters.JsonFormatter),
                        StatusCode = HttpStatusCode.OK
                    };
                }

                resultado.Clear();
            }
            catch (Exception ex)
            {
                var objResponse = new Response
                {
                    mensaje = CONS.Constantes.ERROR_mensaje,
                    error = CONS.Constantes.ERROR_error
                };

                return new HttpResponseMessage
                {
                    Content = new ObjectContent<Response>(objResponse, Configuration.Formatters.JsonFormatter),
                    StatusCode = HttpStatusCode.OK
                };

            }

            return new HttpResponseMessage
            {
                Content = new ObjectContent<EXPAPartidosPorSede>(objPartido, Configuration.Formatters.JsonFormatter),
                StatusCode = HttpStatusCode.OK
            };
        }

        [Route("api/partido/actualizar")]
        [HttpPut]
        public HttpResponseMessage actualizar(PUTPartido putpartido_model)
        {
            Operacion objBdd = new Operacion();
            int filas_afectadas = -1;
            Response objResponse;

            DateTime hora_inicio = DateTime.Parse(putpartido_model.hora_inicio);
            DateTime hora_fin = DateTime.Parse(putpartido_model.hora_fin);

            try
            {

                List<SqlParameter> parametros = new List<SqlParameter>
                    {
                        new SqlParameter() { ParameterName= "@sede_id", Value = putpartido_model.sede_id, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@hora_inicio", Value = hora_inicio, SqlDbType = SqlDbType.DateTime },
                        new SqlParameter() { ParameterName= "@hora_fin", Value = hora_fin, SqlDbType = SqlDbType.DateTime },
                        new SqlParameter() { ParameterName= "@equipo_1_id", Value = putpartido_model.equipo_1_id, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@equipo_2_id", Value = putpartido_model.equipo_2_id, SqlDbType = SqlDbType.Int },

                          new SqlParameter() { ParameterName= "@jugado", Value = putpartido_model.jugado, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@partido_id", Value = putpartido_model.partido_id, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@equipo_1_goles", Value = putpartido_model.equipo_1_goles, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@equipo_2_goles", Value = putpartido_model.equipo_2_goles, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@expaid_equipo_1", Value = putpartido_model.expaid_equipo_1, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@expaid_equipo_2", Value = putpartido_model.expaid_equipo_2, SqlDbType = SqlDbType.Int },
                    };

                filas_afectadas = objBdd.update_insertDataSp(CONS.Constantes.SP_Actualizar_Partido, parametros);

                if (filas_afectadas < 1)
                {
                    objResponse = new Response()
                    {
                        mensaje = CONS.Constantes.ERROR_mensaje,
                        error = CONS.Constantes.ERROR_error
                    };

                    return new HttpResponseMessage
                    {
                        Content = new ObjectContent<Response>(objResponse, Configuration.Formatters.JsonFormatter),
                        StatusCode = HttpStatusCode.OK
                    };
                }
            }
            catch (Exception)
            {
                objResponse = new Response()
                {
                    mensaje = CONS.Constantes.ERROR_mensaje,
                    error = CONS.Constantes.ERROR_error
                };

                return new HttpResponseMessage
                {
                    Content = new ObjectContent<Response>(objResponse, Configuration.Formatters.JsonFormatter),
                    StatusCode = HttpStatusCode.OK
                };
            }

            objResponse = new Response
            {
                mensaje = CONS.Constantes.OK_mensaje_POST,
                error = CONS.Constantes.OK_error_POST
            };

            return new HttpResponseMessage
            {
                Content = new ObjectContent<Response>(objResponse, Configuration.Formatters.JsonFormatter),
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}