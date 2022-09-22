using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACS.Models
{
    public class Sede
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string ubicacion { get; set; }
        public string estadio { get; set; }
        public int capacidad { get; set; }

    }

    public class DetalleSede
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string ubicacion { get; set; }
        public string estadio { get; set; }
        public int capacidad { get; set; }

        public List<PartidosPorSede> partidos { get; set; }

    }
}