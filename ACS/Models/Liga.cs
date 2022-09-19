using System.Collections.Generic;

public class Liga
{
    public int id { get; set; }
    public string tipo_liga { get; set; }
    public string nombre { get; set; }
    public string descripcion { get; set; }
}

public class DetalleLiga 
{
    public int id { get; set; }
    public string tipo_liga { get; set; }
    public string nombre { get; set; }
    public string descripcion { get; set; }
    public List<DetalleUsuarios> usuarios { get; set; }
}