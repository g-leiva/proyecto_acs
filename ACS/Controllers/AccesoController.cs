﻿using System;
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
                objBdd.getDataSp("");
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