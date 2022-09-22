using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS.Models
{
    public class Equipo
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string grupo { get; set; }
    }

    public class NEquipo
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int grupo { get; set; }
    }

    public class DetalleEquipo
    {
        public string nombre { get; set; }
        public string grupo { get; set; }
        public int equipo_goles { get; set; }
    }
}