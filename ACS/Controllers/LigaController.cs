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

namespace ACS.Controllers
{
    public class LigaController : ApiController
    {
        [Route("api/liga/obtener")]
        [HttpGet]
        public HttpResponseMessage obtener()
        {
            List<Liga> objLigas = new List<Liga>();
            Operacion objBdd = new Operacion();
            DataTable resultado = new DataTable();

            try
            {
                resultado = objBdd.getDataSp(CONS.Constantes.SP_Obtener_Ligas);

                if (resultado != null)
                {
                    foreach (DataRow item in resultado.Rows)
                    {
                        objLigas.Add(
                             new Liga()
                             {
                                 id = int.Parse(item["id"].ToString()),
                                 tipo_liga = item["tipo"].ToString(),
                                 nombre = item["nombre"].ToString(),
                                 descripcion = item["descripcion"].ToString(),
                             }
                         );
                    }
                }
                else
                {
                    var objResponse = new Response()
                    {
                        mensaje = CONS.Constantes.LIGAS_No_Existen,
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
                Content = new ObjectContent<List<Liga>>(objLigas, Configuration.Formatters.JsonFormatter),
                StatusCode = HttpStatusCode.OK
            };
        }
        [Route("api/liga/obtener/{liga_id}")]
        [HttpGet]
        public HttpResponseMessage obtener(int liga_id)
        {

            Liga objLiga = new Liga();
            Operacion objBdd = new Operacion();
            DataTable resultado = new DataTable();
            DetalleLiga objDetalleLiga = new DetalleLiga();

            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName= "@id", Value = liga_id, SqlDbType = SqlDbType.Int },
                };

                resultado = objBdd.getDataSp(CONS.Constantes.SP_Obtener_Liga, parametros);

                if (resultado != null)
                {
                    foreach (DataRow item in resultado.Rows)
                    {
                        objLiga = new Liga()
                        {
                            id = int.Parse(item["id"].ToString()),
                            tipo_liga = item["tipo"].ToString(),
                            nombre = item["nombre"].ToString(),
                            descripcion = item["descripcion"].ToString()
                        };
                    }

                    if (objLiga != null)
                    {
                        objDetalleLiga = new DetalleLiga()
                        {
                            id = objLiga.id,
                            tipo_liga = objLiga.tipo_liga,
                            nombre = objLiga.nombre,
                            descripcion = objLiga.descripcion,
                            usuarios = new List<DetalleUsuarios>()
                            
                        };

                        resultado.Clear();
                        parametros.Clear();

                        parametros = new List<SqlParameter>
                        {
                            new SqlParameter() { ParameterName= "@id", Value = liga_id, SqlDbType = SqlDbType.Int },
                        };

                        resultado = objBdd.getDataSp(CONS.Constantes.SP_Obtener_Detalle_Liga, parametros);

                        if (resultado != null)
                        {
                            foreach (DataRow item in resultado.Rows)
                            {
                                objDetalleLiga.usuarios.Add(
                                    new DetalleUsuarios()
                                    {
                                        id = int.Parse(item["id"].ToString()),
                                        nombre = item["nombre"].ToString(),
                                        correo = item["correo"].ToString(),
                                        rol = item["usuario_rol"].ToString(),
                                        puntos = int.Parse(item["usuario_puntos"].ToString()),
                                        posicion = int.Parse(item["posicion"].ToString()),
                                    }
                                 );
                            }
                        }
                    }
                    else
                    {
                        var objResponse = new Response()
                        {
                            mensaje = CONS.Constantes.LIGAS_No_Existente,
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
                        mensaje = CONS.Constantes.LIGAS_No_Existente,
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
                Content = new ObjectContent<DetalleLiga>(objDetalleLiga, Configuration.Formatters.JsonFormatter),
                StatusCode = HttpStatusCode.OK
            };
        }

        [Route("api/liga/crear")]
        [HttpPost]
        public HttpResponseMessage crear(Liga liga_model)
        {

            Operacion objBdd = new Operacion();
            int filas_afectadas = -1;
            Response objResponse;

            try
            {

                List<SqlParameter> parametros = new List<SqlParameter>
                    {
                        new SqlParameter() { ParameterName= "@nombre", Value = liga_model.nombre, SqlDbType = SqlDbType.VarChar },
                        new SqlParameter() { ParameterName= "@tipo", Value = liga_model.tipo_liga, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@descripcion", Value = liga_model.descripcion, SqlDbType = SqlDbType.VarChar },
                    };

                filas_afectadas = objBdd.update_insertDataSp(CONS.Constantes.SP_Crear_Liga, parametros);

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

        [Route("api/liga/actualizar")]
        [HttpPut]
        public HttpResponseMessage actualizar(Liga liga_model)
        {
            Operacion objBdd = new Operacion();
            int filas_afectadas = -1;
            Response objResponse;

            try
            {

                List<SqlParameter> parametros = new List<SqlParameter>
                    {
                        new SqlParameter() { ParameterName= "@id", Value = liga_model.id, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@nombre", Value = liga_model.nombre, SqlDbType = SqlDbType.VarChar },
                        new SqlParameter() { ParameterName= "@tipo", Value = liga_model.tipo_liga, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@descripcion", Value = liga_model.descripcion, SqlDbType = SqlDbType.VarChar },
                    };

                filas_afectadas = objBdd.update_insertDataSp(CONS.Constantes.SP_Actualizar_Liga, parametros);

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

        [Route("api/liga/asignar")]
        [HttpPost]
        public HttpResponseMessage asignar(LigaXUsuario ligaxusuario_model)
        {

            Operacion objBdd = new Operacion();
            int filas_afectadas = -1;
            Response objResponse;

            try
            {

                List<SqlParameter> parametros = new List<SqlParameter>
                    {
                        new SqlParameter() { ParameterName= "@usuario_id", Value = ligaxusuario_model.usuario_id, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@usuario_rol", Value = ligaxusuario_model.usuario_rol, SqlDbType = SqlDbType.TinyInt },
                        new SqlParameter() { ParameterName= "@usuario_puntos", Value = ligaxusuario_model.usuario_puntos, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@liga_id", Value = ligaxusuario_model.liga_id, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@posicion", Value = ligaxusuario_model.posicion, SqlDbType = SqlDbType.Int },
                    };

                filas_afectadas = objBdd.update_insertDataSp(CONS.Constantes.SP_Asignar_Liga, parametros);

                if (filas_afectadas < 1)
                {
                    objResponse = new Response()
                    {
                        mensaje = CONS.Constantes.ERROR_ASIGNARmensaje,
                        error = CONS.Constantes.ERROR_ASGINARerror
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

        [Route("api/liga/actualizarAsignacion")]
        [HttpPut]
        public HttpResponseMessage actualizar(LigaXUsuario ligaxusuario_model)
        {
            Operacion objBdd = new Operacion();
            int filas_afectadas = -1;
            Response objResponse;

            try
            {

                List<SqlParameter> parametros = new List<SqlParameter>
                    {
                        new SqlParameter() { ParameterName= "@usuario_id", Value = ligaxusuario_model.usuario_id, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@usuario_puntos", Value = ligaxusuario_model.usuario_puntos, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@liga_id", Value = ligaxusuario_model.liga_id, SqlDbType = SqlDbType.Int },
                        new SqlParameter() { ParameterName= "@posicion", Value = ligaxusuario_model.posicion, SqlDbType = SqlDbType.Int },
                   };

                filas_afectadas = objBdd.update_insertDataSp(CONS.Constantes.SP_Actualizar_Asignacion, parametros);

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