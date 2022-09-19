
    public class Usuario
    {
    public int id { get; set; }
    public string nombre { get; set; }
    public string password { get; set; }
    public string correo { get; set; }
}

public class DetalleUsuarios 
{
    public int id { get; set; }
    public string nombre { get; set; }
    public string correo { get; set; }
    public string rol { get; set; }
    public int puntos { get; set; }
    public int posicion { get; set; }
}