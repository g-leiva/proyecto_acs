using System.Collections.Generic;

public class LigaUsuario : Response
{
    public List<Liga> Ligas { get; set; }
}

public class LigaXUsuario
{
    public int id { get; set; }
    public int usuario_id { get; set; }
    public int usuario_rol { get; set; }
    public int usuario_puntos { get; set; }
    public int liga_id { get; set; }
    public int posicion { get; set; }
}