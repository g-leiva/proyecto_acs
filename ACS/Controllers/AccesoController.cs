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
    public class AccesoController : ApiController
    {
        [Route("api/autenticar/autenticar")]
        [HttpGet]
        public HttpResponseMessage autenticar(Usuario usuario_model)
        {

            Autenticacion objAutenticacion = new Autenticacion();
            LigaUsuario objLigaUsuario = new LigaUsuario();
            Operacion objBdd = new Operacion();
            DataTable resultado = new DataTable();

            try
            {
                // AUTENTICACION DE USUARIO
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    new SqlParameter() { ParameterName= "@nombre", Value = usuario_model.nombre, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter() { ParameterName= "@password", Value = usuario_model.password, SqlDbType = SqlDbType.VarChar },
                };

                resultado = objBdd.getDataSp(CONS.Constantes.SP_AutenticarUsuario, parametros);

                foreach (DataRow item in resultado.Rows)
                {
                    objAutenticacion = new Autenticacion
                    {
                        usuario_id = int.Parse(item["ID"].ToString()),
                        mensaje = item["MENSAJE"].ToString(),
                        error = int.Parse(item["ERROR"].ToString())
                    };
                }

                parametros.Clear();
                resultado.Clear();

                // CONSULTA DE LIGAS DEL USUARIO
                if (!objAutenticacion.error.ToString().Equals(CONS.Constantes.ERROR_error.ToString()))
                {
                    parametros = new List<SqlParameter>
                    {
                        new SqlParameter() { ParameterName= "@usuario_id", Value = objAutenticacion.usuario_id, SqlDbType = SqlDbType.Int }
                    };

                    resultado = objBdd.getDataSp(CONS.Constantes.SP_Ligas_x_Usuario, parametros);
                    
                    if (resultado == null)
                    {
                        var objResponse = new Response
                        {
                            mensaje = CONS.Constantes.LIGAS_UsuarioSinLigas,
                            error = CONS.Constantes.ERROR_error
                        };

                        return new HttpResponseMessage
                        {
                            Content = new ObjectContent<Response>(objResponse, Configuration.Formatters.JsonFormatter),
                            StatusCode = HttpStatusCode.OK
                        };
                    }

                    foreach (DataRow item in resultado.Rows)
                    {
                        objLigaUsuario.Ligas.Add(
                            new Liga()
                            {
                                id = int.Parse(item["LIGA_ID"].ToString()),
                                tipo_liga = item["TIPO_LIGA"].ToString(),
                                nombre = item["NOMBRE_LIGA"].ToString(),
                                descripcion = item["DESCRIPCION_LIGA"].ToString(),
                            }
                        );                                                
                    }

                    objLigaUsuario.error = CONS.Constantes.OK_error;
                    objLigaUsuario.mensaje = CONS.Constantes.OK_mensaje;

                    parametros.Clear();
                    resultado.Clear();

                }
                else
                {
                    return new HttpResponseMessage
                    {
                        Content = new ObjectContent<Autenticacion>(objAutenticacion, Configuration.Formatters.JsonFormatter),
                        StatusCode = HttpStatusCode.OK
                    };
                }
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
                Content = new ObjectContent<LigaUsuario>(objLigaUsuario, Configuration.Formatters.JsonFormatter),
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}