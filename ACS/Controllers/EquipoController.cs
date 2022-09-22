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
    public class EquipoController : ApiController
    {
        [Route("api/equipo/obtener")]
        [HttpGet]
        public HttpResponseMessage obtener()
        {
            List<Equipo> objEquipos = new List<Equipo>();
            Operacion objBdd = new Operacion();
            DataTable resultado = new DataTable();

            try
            {
                resultado = objBdd.getDataSp(CONS.Constantes.SP_Obtener_Equipos);

                if (resultado != null)
                {
                    foreach (DataRow item in resultado.Rows)
                    {
                        objEquipos.Add(
                             new Equipo()
                             {
                                 id = int.Parse(item["id"].ToString()),
                                 nombre = item["nombre"].ToString(),
                                 grupo = item["grupo"].ToString()
                             }
                         );
                    }
                }
                else
                {
                    var objResponse = new Response()
                    {
                        mensaje = CONS.Constantes.EQUIPOS_No_Existen,
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
                Content = new ObjectContent<List<Equipo>>(objEquipos, Configuration.Formatters.JsonFormatter),
                StatusCode = HttpStatusCode.OK
            };
        }
        [Route("api/equipo/crear")]
        [HttpPost]
        public HttpResponseMessage crear(NEquipo equipo_model)
        {

            Operacion objBdd = new Operacion();
            int filas_afectadas = -1;
            Response objResponse;

            try
            {

                List<SqlParameter> parametros = new List<SqlParameter>
                    {
                        new SqlParameter() { ParameterName= "@nombre", Value = equipo_model.nombre, SqlDbType = SqlDbType.VarChar },
                        new SqlParameter() { ParameterName= "@grupo_id", Value = equipo_model.grupo, SqlDbType = SqlDbType.Int }
                    };

                filas_afectadas = objBdd.update_insertDataSp(CONS.Constantes.SP_Crear_Equipo, parametros);

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
