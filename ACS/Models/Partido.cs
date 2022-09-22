using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS.Models
{
    public class Partido
    {
        public int id { get; set; }
        public DateTime hora_inicio { get; set; }
        public DateTime hora_fin { get; set; }
        public int jugado { get; set; }
    }

    public class PartidosPorSede
    {
        public int id { get; set; }
        public DateTime hora_inicio { get; set; }
        public DateTime hora_fin { get; set; }
        public int jugado { get; set; }
        public List<DetalleEquipo> equipos { get; set; }

    }
}