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
        [Route("api/liga/obtener/{int}")]
        [HttpGet]
        public HttpResponseMessage obtener(int liga_id)
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
        }
        [Route("api/liga/obtener")]
        [HttpGet]
        public HttpResponseMessage obtener(Liga liga_model)
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
        }
        [Route("api/liga/crear")]
        [HttpPost]
        public HttpResponseMessage crear(Liga liga_model)
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
        }
        [Route("api/liga/actualizar")]
        [HttpPost]
        public HttpResponseMessage actualizar(Liga liga_model)
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
