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

    public class PPartido
    {
        public int sede_id { get; set; }
        public string hora_inicio { get; set; }
        public string hora_fin { get; set; }
        public int equipo_1_id { get; set; }
        public int equipo_2_id { get; set; }
    }

    public class PUTPartido: PPartido
    {
        public int jugado { get; set; }
        public int partido_id { get; set; }
        public int equipo_1_goles { get; set; }
        public int equipo_2_goles { get; set; }
        public int expaid_equipo_1 { get; set; }
        public int expaid_equipo_2 { get; set; }

    }

    public class EXPAPartidosPorSede
    {
        public int sede_id { get; set; }
        public int id { get; set; }
        public DateTime hora_inicio { get; set; }
        public DateTime hora_fin { get; set; }
        public int jugado { get; set; }
        public List<EXPADetalleEquipo> equipos { get; set; }
    }
}