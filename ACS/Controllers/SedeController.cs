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
    public class SedeController : ApiController
    {

        [Route("api/sede/obtener")]
        [HttpGet]
        public HttpResponseMessage obtener()
        {
            List<Sede> objSedes = new List<Sede>();
            Operacion objBdd = new Operacion();
            DataTable resultado = new DataTable();

            try
            {
                resultado = objBdd.getDataSp(CONS.Constantes.SP_Obtener_Sedes);

                if (resultado != null)
                {
                    foreach (DataRow item in resultado.Rows)
                    {
                        objSedes.Add(
                             new Sede()
                             {
                                 id = int.Parse(item["id"].ToString()),
                                 nombre = item["nombre"].ToString(),
                                 ubicacion = item["ubicacion"].ToString(),
                                 estadio = item["estadio"].ToString(),
                                 capacidad = int.Parse(item["capacidad"].ToString())
                             }
                         );
                    }
                }
                else
                {
                    var objResponse = new Response()
                    {
                        mensaje = CONS.Constantes.SEDES_No_Existen,
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
                Content = new ObjectContent<List<Sede>>(objSedes, Configuration.Formatters.JsonFormatter),
                StatusCode = HttpStatusCode.OK
            };
        }
        [Route("api/sede/obtener/{sede_id}")]
        [HttpGet]
        public HttpResponseMessage obtener(int sede_id)
        {

            Sede objSede = new Sede();
            Operacion objBdd = new Operacion();
            DataTable resultado = new DataTable();
            DetalleSede objDetalleSede = new DetalleSede();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName= "@id", Value = sede_id, SqlDbType = SqlDbType.Int },
                };

                resultado = objBdd.getDataSp(CONS.Constantes.SP_Obtener_Sede, parametros);

                if (resultado != null)
                {
                    foreach (DataRow item in resultado.Rows)
                    {
                        objSede = new Sede()
                        {
                            id = int.Parse(item["id"].ToString()),
                            nombre = item["nombre"].ToString(),
                            ubicacion = item["ubicacion"].ToString(),
                            estadio = item["estadio"].ToString(),
                            capacidad = int.Parse(item["capacidad"].ToString())
                        };
                    }

                    if (objSede != null)
                    {
                        objDetalleSede = new DetalleSede()
                        {
                            id = objSede.id,
                            nombre = objSede.nombre,
                            ubicacion = objSede.ubicacion,
                            estadio = objSede.estadio,
                            capacidad = objSede.capacidad,
                            partidos = new List<PartidosPorSede>()

                        };

                        resultado.Clear();
                        parametros.Clear();

                        parametros = new List<SqlParameter>
                        {
                            new SqlParameter() { ParameterName= "@id", Value = sede_id, SqlDbType = SqlDbType.Int },
                        };

                        resultado = objBdd.getDataSp(CONS.Constantes.SP_Obtener_Partidos_x_Sede, parametros);

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

                                List<DetalleEquipo> detalleEquipos = new List<DetalleEquipo>();

                                if (equipos != null)
                                {
                                    foreach (DataRow equipo in equipos.Rows)
                                    {
                                        detalleEquipos.Add(
                                            new DetalleEquipo
                                            {
                                                nombre = equipo["nombre"].ToString(),
                                                equipo_goles = int.Parse(equipo["equipo_goles"].ToString()),
                                                grupo = equipo["grupo"].ToString(),

                                            }
                                        );
                                    }
                                }

                                objDetalleSede.partidos.Add(
                                    new PartidosPorSede()
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
                    }
                    else
                    {
                        var objResponse = new Response()
                        {
                            mensaje = CONS.Constantes.SEDES_No_Existen,
                            error = CONS.Constantes.ERROR_error
                        };
                        return new HttpResponseMessage
                        {
                            Content = new ObjectContent<Response>(objResponse, Configuration.Formatters.JsonFormatter),
                            StatusCode = HttpStatusCode.OK
                        };
                    }

                }
                else
                {
                    var objResponse = new Response()
                    {
                        mensaje = CONS.Constantes.SEDES_No_Existen,
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
                Content = new ObjectContent<DetalleSede>(objDetalleSede, Configuration.Formatters.JsonFormatter),
                StatusCode = HttpStatusCode.OK
            };
        }

        [Route("api/sede/crear")]
        [HttpPost]
        public HttpResponseMessage crear(Sede sede_model)
        {

            Operacion objBdd = new Operacion();
            int filas_afectadas = -1;
            Response objResponse;

            try
            {

                List<SqlParameter> parametros = new List<SqlParameter>
                    {
                        new SqlParameter() { ParameterName= "@nombre", Value = sede_model.nombre, SqlDbType = SqlDbType.VarChar },
                        new SqlParameter() { ParameterName= "@ubicacion", Value = sede_model.ubicacion, SqlDbType = SqlDbType.VarChar },
                        new SqlParameter() { ParameterName= "@estadio", Value = sede_model.estadio, SqlDbType = SqlDbType.VarChar },
                        new SqlParameter() { ParameterName= "@capacidad", Value = sede_model.capacidad, SqlDbType = SqlDbType.Int }
                    };

                filas_afectadas = objBdd.update_insertDataSp(CONS.Constantes.SP_Crear_Sede, parametros);

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

        [Route("api/sede/actualizar")]
        [HttpPut]
        public HttpResponseMessage actualizar(Sede sede_model)
        {
            Operacion objBdd = new Operacion();
            int filas_afectadas = -1;
            Response objResponse;

            try
            {

                List<SqlParameter> parametros = new List<SqlParameter>
                    {
                        new SqlParameter() { ParameterName= "@id", Value = sede_model.id, SqlDbType = SqlDbType.Int },
                         new SqlParameter() { ParameterName= "@nombre", Value = sede_model.nombre, SqlDbType = SqlDbType.VarChar },
                        new SqlParameter() { ParameterName= "@ubicacion", Value = sede_model.ubicacion, SqlDbType = SqlDbType.VarChar },
                        new SqlParameter() { ParameterName= "@estadio", Value = sede_model.estadio, SqlDbType = SqlDbType.VarChar },
                        new SqlParameter() { ParameterName= "@capacidad", Value = sede_model.capacidad, SqlDbType = SqlDbType.Int }
                    };

                filas_afectadas = objBdd.update_insertDataSp(CONS.Constantes.SP_Actualizar_Sede, parametros);

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

