using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using ACS.OperacionBDD;
namespace ACS.Controllers
{
    public class AccesoController : ApiController
    {
        [Route("api/autenticar")]
        [HttpGet]
        public HttpResponseMessage autenticar()
        {
            try
            {
                Operacion objBdd = new Operacion();
                DataTable resultado = new DataTable();
                
                objBdd.getDataSp("");

                foreach (DataRow item in resultado.Rows)
                {
                    var usuario = new Usuario
                    {
                        id = int.Parse(item["id"].ToString()),
                        nombre = item["nombre"].ToString(),
                        password = item["password"].ToString(),
                        correo = item["correo"].ToString()
                    };
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}